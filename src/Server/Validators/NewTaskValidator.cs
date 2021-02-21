using BxTestTask.Handlers;
using BxTestTask.Stores;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace BxTestTask.Validators
{
  public class NewTaskValidator : AbstractValidator<NewTaskValidationContext>
  {
    public NewTaskValidator()
    {
      RuleFor(ctx => ctx.NewTask)
        .SetValidator(new TodoTaskEntityValidator());
      
      RuleFor(ctx => ctx)
        .Must(ctx => !ctx.Tasks.Any(existing => existing.Name == ctx.NewTask.Name))
        .WithMessage(ctx => $"Task with name ${ctx.NewTask.Name} already exists")
        .WithErrorCode(Code.VALIDATION)
        .OverridePropertyName(Field.NAME);

      RuleFor(ctx => ctx)
        .Must(ctx => !ctx.Tasks.Any(existing => existing.Id == ctx.NewTask.Id))
        .WithMessage(ctx => $"Task with id ${ctx.NewTask.Id} already exists")
        .WithErrorCode(Code.VALIDATION)
        .OverridePropertyName(Field.ID);
    }
  }

  public class NewTaskValidationContext
  {
    public IEnumerable<TodoTaskEntity> Tasks { get; init; }
    public TodoTaskEntity NewTask { get; init; }
  }
}