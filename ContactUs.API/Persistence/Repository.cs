using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Persistence
{
  public class Repository : IRepository
  {
    private readonly ContactUsContext _db;
    public Repository(ContactUsContext db)
    {
      _db = db;
    }

    #region Commands [Add-Update-Delete]
    public T Add<T>(T item) where T : class
    {
      return _db.Set<T>().Add(item).Entity;
    }
    public void AddRange<T>(List<T> range) where T : class
    {
      _db.Set<T>().AddRange(range);
    }
    public List<T> AddAndGetRange<T>(List<T> range) where T : class
    {
      return range
        .Select(obj => _db.Set<T>().Add(obj).Entity)
        .ToList();
    }

    public T Update<T, TKey>(T item) where T : Entity<TKey>
    {
      item.ModifiedAt = DateTime.UtcNow.AddHours(2);
      return _db.Set<T>().Update(item).Entity;
    }
    public void UpdateRange<T, TKey>(List<T> range) where T : Entity<TKey>
    {
      range.ForEach(item => item.ModifiedAt = DateTime.UtcNow.AddHours(2));
      _db.Set<T>().UpdateRange(range);
    }
    public List<T> UpdateAndGetRange<T, TKey>(List<T> range) where T : Entity<TKey>
    {
      range.ForEach(item => item.ModifiedAt = DateTime.UtcNow.AddHours(2));
      return range
        .Select(obj => _db.Set<T>().Update(obj).Entity)
        .ToList();
    }

    public void Delete<T>(T item) where T : class
    {
      _db.Set<T>().Remove(item);
    }
    public void DeleteRange<T>(List<T> range) where T : class
    {
      _db.Set<T>().RemoveRange(range);
    }
    public bool GetOneAndDelete<T>(Expression<Func<T, bool>> where) where T : class
    {
      var store = _db.Set<T>();
      T item = store.Where(where).FirstOrDefault();
      if (item != null)
      {
        store.Remove(item);
        return true;
      }
      return false;
    }
    public bool GetListAndDelete<T>(Expression<Func<T, bool>> where) where T : class
    {
      var store = _db.Set<T>();
      List<T> items = store.Where(where).ToList();
      if (items.Count != 0)
      {
        store.RemoveRange(items);
        return true;
      }
      return false;
    }
    public void SoftDelete<T, TKey>(T item) where T : Entity<TKey>
    {
      item.IsDeleted = true;
      _db.Entry(item).State = EntityState.Modified;
    }
    public void SoftDeleteRange<T, TKey>(List<T> range) where T : Entity<TKey>
    {
      range.ForEach(item =>
      {
        item.IsDeleted = true;
        _db.Entry(item).State = EntityState.Modified;
      });
    }
    public bool GetOneAndSoftDelete<T, TKey>(Expression<Func<T, bool>> where) where T : Entity<TKey>
    {
      T item = _db.Set<T>().Where(where).FirstOrDefault();
      if (item != null)
      {
        SoftDelete<T, TKey>(item);
        return true;
      }
      return false;
    }
    public bool GetListAndSoftDelete<T, TKey>(Expression<Func<T, bool>> where) where T : Entity<TKey>
    {
      List<T> items = _db.Set<T>().Where(where).ToList();
      if (items.Count != 0)
      {
        SoftDeleteRange<T, TKey>(items);
        return true;
      }
      return false;
    }

    #endregion

    #region Query [Select]
    public T Find<T, TKey>(TKey id) where T : Entity<TKey>
    {
      return _db.Set<T>().Find(id);
    }
    public T GetOne<T>(Expression<Func<T, bool>> where) where T : class
    {
      return _db.Set<T>().AsNoTracking().SingleOrDefault(where);
    }
    public T GetOne<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes) where T : class
    {
      var query = _db.Set<T>().AsNoTracking();
      foreach (var include in includes)
        query = query.Include(include);
      return query.Where(where).FirstOrDefault();
    }
    public T GetOne<T>(Expression<Func<T, bool>> where, params string[] includes) where T : class
    {
      var query = _db.Set<T>().AsNoTracking();
      foreach (var include in includes)
        query = query.Include(include);
      return query.Where(where).FirstOrDefault();
    }

    public List<T> GetAll<T, TKey>() where T : Entity<TKey>
    {
      return _db.Set<T>().AsNoTracking().ToList();
    }
    public List<T> GetList<T, TKey>(Expression<Func<T, bool>> where) where T : Entity<TKey>
    {
      return _db.Set<T>().AsNoTracking().Where(where).ToList();
    }
    public List<T> GetList<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes) where T : class
    {
      var query = _db.Set<T>().AsNoTracking();
      foreach (var include in includes)
        query = query.Include(include);
      return query.Where(where).ToList();
    }
    public List<T> GetList<T>(Expression<Func<T, bool>> where, params string[] includes) where T : class
    {
      var query = _db.Set<T>().AsNoTracking();
      foreach (var include in includes)
        query = query.Include(include);
      return query.Where(where).ToList();
    }

    public IQueryable<T> GetQuery<T>() where T : class
    {
      return _db.Set<T>().AsNoTracking();
    }
    public IQueryable<T> GetQuery<T>(Expression<Func<T, bool>> where) where T : class
    {
      return _db.Set<T>().AsNoTracking().Where(where);
    }
    public PageResult<T> GetPage<T>(IQueryable<T> query, int pageSize, int pageNumber) where T : class
    {
      var count = query.Count();
      return new PageResult<T>
      {
        PageItems = query.Skip(pageSize * (pageNumber - 1))
                          .Take(pageSize)
                          .ToList(),
        TotalItems = count,
        TotalPages = (int)Math.Ceiling((decimal)count / pageSize),
      };
    }

    public int Count<T>() where T : class
    {
      return _db.Set<T>().Count();
    }
    public int Count<T>(Expression<Func<T, bool>> where) where T : class
    {
      return _db.Set<T>().Count(where);
    }

    #endregion

    #region [UnitOfEWork]
    public void Attach<T>(T entity) where T : class
    {
      _db.Set<T>().Attach(entity);
    }
    public void SetState<T>(T entity, string state) where T : class
    {
      switch (state)
      {
        case "Added":
          _db.Entry(entity).State = EntityState.Added;
          break;
        case "Modified":
          _db.Entry(entity).State = EntityState.Modified;
          break;
        case "Deleted":
          _db.Entry(entity).State = EntityState.Deleted;
          break;
        default:
          _db.Entry(entity).State = EntityState.Unchanged;
          break;
      }
    }
    public int SaveChanges()
    {
      return _db.SaveChanges();
    }

    #endregion

  }
}
