using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BxTestTask.Validators
{
  public class OverrideModelValidationAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid)
      {
        context.Result = new BadRequestObjectResult(new ValidationResultModel(context.ModelState));
      }
    }
  }
  
  public class ValidationError
  {
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Field { get; }

    public string Message { get; }

    public ValidationError(string field, string message)
    {
      Field = field != string.Empty ? field : null;
      Message = message;
    }
  }

  public class ValidationResultModel
  {
    public string Message { get; } 

    public List<ValidationError> Errors { get; }

    public ValidationResultModel(ModelStateDictionary modelState)
    {
      Message = "Validation Failed";
      Errors = modelState.Keys
        .SelectMany(key => modelState[key].Errors
          .Select(x => new ValidationError(key, x.ErrorMessage)))
        .ToList();
    }
  }
}