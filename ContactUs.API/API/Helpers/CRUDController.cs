using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Persistence;
using Domain;

namespace API
{
  [ApiController]
  [Route("api/[controller]/[action]")]
  public class CRUDController<T, TKey, TDto> : ControllerBase
    where T : Entity<TKey>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;
    private readonly DbActions _dbActions;

    public CRUDController(IRepository repository, IMapper mapper, IConfiguration config)
    {
      _repository = repository;
      _mapper = mapper;
      _dbActions = config.GetValue<DbActions>($"DbActions:{typeof(T).Name}");
    }

    #region mapper methods
    private IActionResult mapEntity(T entity)
    {
      if (typeof(T).Name != typeof(TDto).Name)
        return Ok(_mapper.Map<TDto>(entity));
      return Ok(entity);
    }
    private IActionResult mapList(List<T> list)
    {
      if (typeof(T).Name != typeof(TDto).Name)
        return Ok(_mapper.Map<List<TDto>>(list));
      return Ok(list);
    }
    private IActionResult mapPageResult(PageResult<T> pageResult)
    {
      if (typeof(T).Name != typeof(TDto).Name)
      {
        return Ok(new PageResult<TDto>()
        {
          PageItems = _mapper.Map<List<TDto>>(pageResult.PageItems),
          TotalItems = pageResult.TotalItems,
          TotalPages = pageResult.TotalPages
        });
      }
      return Ok(pageResult);
    }

    #endregion

    #region Queries
    /// <summary>
    /// [controller]/List/all
    /// listType values (all/deleted/existed)
    /// </summary>
    /// <returns>List of T</returns>
    [HttpGet("{type}")]
    public IActionResult List(string type = "existed")
    {
      switch (type.ToLower())
      {
        case "all":
          return mapList(_repository.GetAll<T, TKey>());
        case "deleted":
          return mapList(_repository.GetList<T, TKey>(e => e.IsDeleted));
        default:
          return mapList(_repository.GetList<T, TKey>(e => !e.IsDeleted));
      }
    }

    /// <summary>
    /// [controller]/ListPage/all?pagesize=25&pageNumber=4
    /// listType values (all/deleted/existed)
    /// </summary>
    /// <returns>PageResult<T></returns>
    [HttpGet("{listType}")]
    public IActionResult ListPage(string listType = "existed", int pageSize = 10, int pageNumber = 1)
    {
      IQueryable<T> query;
      switch (listType.ToLower())
      {
        case "all":
          query = _repository.GetQuery<T>();
          break;
        case "deleted":
          query = _repository.GetQuery<T>(e => e.IsDeleted);
          break;
        default:
          query = _repository.GetQuery<T>(e => !e.IsDeleted);
          break;
      }
      return mapPageResult(_repository.GetPage(query, pageSize, pageNumber));
    }

    /// <summary>
    /// [controller]/QueryList?where=&select=&orderBy=
    /// get query list data by applying dynamic query
    /// </summary>
    /// <returns>List<T></returns>
    [HttpGet]
    public IActionResult QueryList(string where, string orderBy, string select, int? pageSize, int? pageNumber)
    {
      if (where == null && select == null && orderBy == null)
        return BadRequest(new Error { Message = "Must supply at least one of the following : [filters] and/or [fields] and/or [orderBy]" });

      IQueryable<T> query = _repository.GetQuery<T>();

      if (where != null)
        query = query.Where(where);
      if (orderBy != null)
        query = query.OrderBy(orderBy.RemoveEmptyElements(','));
      if (select != null)
        query = query.OrderBy(select.RemoveEmptyElements(','));

      if (pageSize != null && pageNumber != null)
        return mapPageResult(_repository.GetPage<T>(query, pageSize ?? 10, pageNumber ?? 1));

      return mapList(query.ToList());
    }

    /// <summary>
    /// [controller]/Find/1
    /// </summary>
    /// <returns>ActionResult<T></returns>
    [HttpGet("{id}")]
    public IActionResult Find(TKey id)
    {
      var entity = _repository.Find<T, TKey>(id);
      if (entity == null)
        return BadRequest(new Error { Message = "Item not Found ..." });
      return mapEntity(entity);
    }

    /// <summary>
    /// [controller]/FindList/1,2,3
    /// </summary>
    /// <returns>ActionResult<List<T>></returns>
    [HttpGet("{ids}")]
    public IActionResult FindList(string ids)
    {
      if (ids == null)
        return BadRequest(new Error { Message = "Must supply comma separated string of ids" });

      var _ids = ids.SplitAndRemoveEmpty(',')
                    .ToList();
      var result = _repository.GetList<T, TKey>(e => _ids.Contains(e.Id.ToString()));
      return mapList(result);
    }

    /// <summary>
    /// [controller]/Lookup
    /// </summary>
    /// <returns>ActionResult<List<Lookup<TKey>>></returns>
    [HttpGet]
    public IActionResult Lookup(string propName)
    {
      var result = _repository
        .GetAll<T, TKey>()
        .Select(e => new Lookup<TKey>()
        {
          Id = e.Id,
          Name = String.IsNullOrWhiteSpace(propName) ? e.Name : e.GetValue(propName).ToString()
        })
        .ToList();
      return Ok(result);
    }

    #endregion

    #region Commands

    /// <summary>
    /// [controller]/AddItem
    /// </summary>
    /// <returns>ActionResult<T></returns>
    [HttpPost]
    public IActionResult AddItem(T item)
    {
      if (_dbActions != null && !_dbActions.Add)
        return BadRequest(new Error() { Message = "Oops! Action not allowed !!" });

      T dbItem = _repository.Add<T>(item);
      _repository.SaveChanges();
      return mapEntity(dbItem);
    }

    /// <summary>
    /// [controller]/AddItems
    /// </summary>
    /// <returns>ActionResult<List<T>></returns>
    [HttpPost]
    public IActionResult AddItems(List<T> items)
    {
      if (_dbActions != null && !_dbActions.Add)
        return BadRequest(new Error() { Message = "Oops! Action not allowed !!" });

      var dbItems = _repository.AddAndGetRange<T>(items);
      _repository.SaveChanges();
      return mapList(dbItems);
    }

    /// <summary>
    /// [controller]/UpdateItem
    /// </summary>
    /// <returns>ActionResult<T></returns>
    [HttpPost]
    public IActionResult UpdateItem(T item)
    {
      if (_dbActions != null && !_dbActions.Update)
        return BadRequest(new Error() { Message = "Oops! Action not allowed !!" });

      T dbItem = _repository.Update<T, TKey>(item);
      _repository.SaveChanges();
      return mapEntity(dbItem);
    }

    /// <summary>
    /// [controller]/UpdateItems
    /// </summary>
    /// <returns>ActionResult<List<T>></returns>
    [HttpPost]
    public IActionResult UpdateItems(List<T> items)
    {
      if (_dbActions != null && !_dbActions.Update)
        return BadRequest(new Error() { Message = "Oops! Action not allowed !!" });

      var dbItems = _repository.UpdateAndGetRange<T, TKey>(items);
      _repository.SaveChanges();
      return mapList(dbItems);
    }

    /// <summary>
    /// [controller]/DeleteItem
    /// </summary>
    /// <returns>ActionResult<bool></returns>
    [HttpPost]
    public ActionResult<bool> DeleteItem(T item)
    {
      if (_dbActions != null && !_dbActions.Delete)
        return BadRequest(new Error() { Message = "Oops! Action not allowed !!" });

      _repository.Delete<T>(item);
      _repository.SaveChanges();
      return Ok(true);
    }

    /// <summary>
    /// [controller]/DeleteItems
    /// </summary>
    /// <returns>ActionResult<bool></returns>
    [HttpPost]
    public ActionResult<bool> DeleteItems(List<T> items)
    {
      if (_dbActions != null && !_dbActions.Delete)
        return BadRequest(new Error() { Message = "Oops! Action not allowed !!" });

      _repository.DeleteRange<T>(items);
      _repository.SaveChanges();
      return Ok(true);
    }

    /// <summary>
    /// [controller]/SoftDeleteItem
    /// </summary>
    /// <returns>ActionResult<T></returns>
    [HttpPost]
    public ActionResult<bool> SoftDeleteItem(T item)
    {
      if (_dbActions != null && !_dbActions.SoftDelete)
        return BadRequest(new Error() { Message = "Oops! Action not allowed !!" });

      _repository.SoftDelete<T, TKey>(item);
      _repository.SaveChanges();
      return Ok(true);
    }

    /// <summary>
    /// [controller]/SoftDeleteItems
    /// </summary>
    /// <returns>ActionResult<bool></returns>
    [HttpPost]
    public ActionResult<bool> SoftDeleteItems(List<T> items)
    {
      if (_dbActions != null && !_dbActions.SoftDelete)
        return BadRequest(new Error() { Message = "Oops! Action not allowed !!" });

      _repository.SoftDeleteRange<T, TKey>(items);
      _repository.SaveChanges();
      return Ok(true);
    }

    #endregion
  }
}
