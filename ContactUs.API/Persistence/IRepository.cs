using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain;

namespace Persistence
{
  public interface IRepository
  {
    #region Query [Select]
    T Find<T, TKey>(TKey id) where T : Entity<TKey>;
    T GetOne<T>(Expression<Func<T, bool>> where) where T : class;
    T GetOne<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes) where T : class;
    T GetOne<T>(Expression<Func<T, bool>> where, params string[] includes) where T : class;

    List<T> GetAll<T, TKey>() where T : Entity<TKey>;
    List<T> GetList<T, TKey>(Expression<Func<T, bool>> where) where T : Entity<TKey>;
    List<T> GetList<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes) where T : class;
    List<T> GetList<T>(Expression<Func<T, bool>> where, params string[] includes) where T : class;

    IQueryable<T> GetQuery<T>() where T : class;
    IQueryable<T> GetQuery<T>(Expression<Func<T, bool>> where) where T : class;
    PageResult<T> GetPage<T>(IQueryable<T> query, int pageSize, int pageNumber) where T : class;

    int Count<T>() where T : class;
    int Count<T>(Expression<Func<T, bool>> where) where T : class;
    #endregion

    #region Commands [Add-Update-Delete]
    T Add<T>(T item) where T : class;
    void AddRange<T>(List<T> range) where T : class;
    List<T> AddAndGetRange<T>(List<T> range) where T : class;

    T Update<T, TKey>(T item) where T : Entity<TKey>;
    void UpdateRange<T, TKey>(List<T> range) where T : Entity<TKey>;
    List<T> UpdateAndGetRange<T, TKey>(List<T> range) where T : Entity<TKey>;

    void Delete<T>(T item) where T : class;
    void DeleteRange<T>(List<T> range) where T : class;
    bool GetOneAndDelete<T>(Expression<Func<T, bool>> where) where T : class;
    bool GetListAndDelete<T>(Expression<Func<T, bool>> where) where T : class;
    void SoftDelete<T, TKey>(T item) where T : Entity<TKey>;
    void SoftDeleteRange<T, TKey>(List<T> range) where T : Entity<TKey>;
    bool GetOneAndSoftDelete<T, TKey>(Expression<Func<T, bool>> where) where T : Entity<TKey>;
    bool GetListAndSoftDelete<T, TKey>(Expression<Func<T, bool>> where) where T : Entity<TKey>;

    #endregion

    #region [UnitOfEWork]
    void Attach<T>(T entity) where T : class;
    void SetState<T>(T entity, string state) where T : class;
    int SaveChanges();

    #endregion

  }
}
