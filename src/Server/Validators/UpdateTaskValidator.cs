using System.Collections.Generic;
using System.Linq;
using BxTestTask.Stores;
using FluentValidation;

namespace BxTestTask.Validators
{
  public class UpdateTaskValidator : AbstractValidator<UpdateTaskValidationContext>
  {
    public UpdateTaskValidator()
    {
      RuleFor(ctx => ctx.UpdatedTask)
        .SetValidator(new TodoTaskEntityValidator());

      RuleFor(ctx => ctx.PristineTask)
        .NotNull()
        .WithMessage(ctx => $"Task with id {ctx.UpdatedTask.Id} does not exist")
        .WithErrorCode(Code.NOT_FOUND)
        .OverridePropertyName(Field.ID);
      
      RuleFor(ctx => ctx)
        .Must(ctx => !ctx.Tasks.Any(existing => existing.Name == ctx.UpdatedTask.Name))
        .WithMessage(ctx => $"Update to name {ctx.UpdatedTask.Name} would violate name uniqueness")
        .WithErrorCode(Code.VALIDATION)
        .OverridePropertyName(Field.NAME);
    }
  }
  
  public class UpdateTaskValidationContext
  {
    public TodoTaskEntity PristineTask { get; init; }
    public IEnumerable<TodoTaskEntity> Tasks { get; init; }
    public TodoTaskEntity UpdatedTask { get; init; }
  }
}