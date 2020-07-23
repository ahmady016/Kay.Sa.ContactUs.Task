using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API
{
  public class ApiExceptionFilter : ExceptionFilterAttribute
  {
    public override void OnException(ExceptionContext actionContext)
    {
      string message = $"{actionContext.Exception.Message}|_|{actionContext.Exception.InnerException?.Message}|_|{actionContext.Exception.InnerException?.InnerException?.Message}";
      actionContext.Result = new BadRequestObjectResult(new Error { Message = message });
    }
  }
}
