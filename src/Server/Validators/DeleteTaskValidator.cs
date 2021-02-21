using System;
using BxTestTask.Stores;
using FluentValidation;

namespace BxTestTask.Validators
{
  public class DeleteTaskValidator : AbstractValidator<DeleteTaskValidationContext>
  {
    public DeleteTaskValidator()
    {
      RuleFor(ctx => ctx.TaskToDelete)
        .NotNull()
        .WithMessage(ctx => $"Unable to delete task {ctx.Id}, task not found")
        .WithErrorCode(Code.NOT_FOUND)
        .OverridePropertyName(Field.ID);

      RuleFor(ctx => ctx.TaskToDelete)
        .SetValidator(new TodoTaskEntityValidator())
        .When(ctx => ctx.TaskToDelete != null);

      RuleFor(ctx => ctx.TaskToDelete)
        .Must(task => task.Status == TodoTaskEntityStatus.Completed)
        .WithMessage(ctx => $"Unable to delete task {ctx.Id} in status {ctx.TaskToDelete.Status}")
        .WithErrorCode(Code.VALIDATION)
        .OverridePropertyName(Field.STATUS)
        .When(ctx => ctx.TaskToDelete != null);
    }
  }

  public class DeleteTaskValidationContext
  {
    public Guid Id { get; init; }
    public TodoTaskEntity TaskToDelete { get; init; }
  }
}