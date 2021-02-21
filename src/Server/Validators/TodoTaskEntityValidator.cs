using BxTestTask.Stores;
using FluentValidation;
using System;

namespace BxTestTask.Validators
{
  public class TodoTaskEntityValidator : AbstractValidator<TodoTaskEntity>
  {
    public TodoTaskEntityValidator()
    {
      RuleFor(entity => entity.Id)
        .Must(id => id != Guid.Empty)
        .WithMessage($"{nameof(TodoTaskEntity)} should have non-empty id")
        .WithErrorCode(Code.VALIDATION)
        .OverridePropertyName(Field.ID);

      RuleFor(entity => entity.Name)
        .Must(name => !string.IsNullOrWhiteSpace(name))
        .WithMessage($"{nameof(TodoTaskEntity)} should have non-empty name")
        .WithErrorCode(Code.VALIDATION)
        .OverridePropertyName(Field.NAME);

      RuleFor(entity => entity.Priority)
        .Must(priority => priority > 0)
        .WithMessage($"{nameof(TodoTaskEntity)} should have non-zero, positive priority")
        .WithErrorCode(Code.VALIDATION)
        .OverridePropertyName(Field.PRIORITY);
    }
  }
}