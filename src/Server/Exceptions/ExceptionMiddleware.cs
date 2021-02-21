using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FluentValidation;
using System.Linq;
using BxTestTask.Validators;

namespace BxTestTask.Exceptions
{
  public class ExceptionMiddleware
  {
    private readonly ILogger<ExceptionMiddleware> logger;
    private readonly RequestDelegate next;
    public ExceptionMiddleware(
      ILogger<ExceptionMiddleware> logger,
      RequestDelegate next)
    {
      this.logger = logger;
      this.next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await next(httpContext);
      }
      catch (ValidationException validation)
      {
        logger.LogError(validation, validation.Message);
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = 
          validation.Errors.Any(e => e.ErrorCode == Code.NOT_FOUND)
            ? (int)HttpStatusCode.NotFound
            : (int)HttpStatusCode.BadRequest;
        await httpContext.Response.WriteAsJsonAsync(new 
        {
          Message = Message.VALIDATION_FAILED,
          Errors = validation.Errors
            .Select(error => new
            {
              Field = error.PropertyName,
              Error = error.ErrorMessage
            })
        });
      }
      catch (Exception ex)
      {
        logger.LogError(ex, ex.Message);
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(new 
        {
          ex.Message
        });
      }
    }
  }
}