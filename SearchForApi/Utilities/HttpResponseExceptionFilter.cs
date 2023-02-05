using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SearchForApi.Models.Dtos;

namespace SearchForApi.Utilities
{
    public class HttpResponseExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var resultStatusCode = (int)HttpStatusCode.OK;
            context.HttpContext.Response.StatusCode = resultStatusCode;
            context.HttpContext.Response.ContentType = "application/json";
            if (context.Exception != null)
            {
                var isCustomException = context.Exception.Data.Contains("code");
                context.Result = new ObjectResult(context.Exception)
                {
                    StatusCode = resultStatusCode,
                    Value = new ResponseDto<string>()
                    {
                        Status = ResponseStatusType.ERROR,
                        Code = !isCustomException ? "unhandled" : (string)context.Exception.Data["code"],
                        Message = !isCustomException ? "Internal Server Error." : (string)context.Exception.Data["message"],
                    }
                };

                Serilog.Log.Error(context.Exception, "type: {type}, message: {message}", context.Exception.GetType().Name, context.Exception.Message);

                context.ExceptionHandled = true;
            }
        }
    }
}

