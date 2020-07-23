using System.Collections.Generic;

namespace Domain
{
  public class PageResult<T>
  {
    public List<T> PageItems { get; set; } = null;
    public long TotalItems { get; set; } = 0;
    public int TotalPages { get; set; } = 0;

  }
}
