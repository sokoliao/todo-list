using System;
using System.Linq;
using System.Threading.Tasks;
using BxTestTask.Stores;
using BxTestTask.Validators;
using FluentValidation;

namespace BxTestTask.Handlers
{
  public class CreateTodoTaskCommandHandler 
    : ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult>
  {
    private readonly ITodoTaskStore store;
    private readonly IValidator<NewTaskValidationContext> validator;

    public CreateTodoTaskCommandHandler(
      ITodoTaskStore store,
      IValidator<NewTaskValidationContext> validator)
    {
      this.store = store;
      this.validator = validator;
    }
    public async Task<CreateTodoTaskResult> HandleAsync(
      CreateTodoTaskCommand command)
    {
      var newTask = command.ToEntity(Guid.NewGuid());

      await store.Set(current => 
      {
        var context = new NewTaskValidationContext
        {
          Tasks = current,
          NewTask = newTask
        };
        validator.ValidateAndThrow(context);
        return context.Tasks.Append(context.NewTask);
      });

      return new CreateTodoTaskResult
      {
        Id = newTask.Id
      };
    }
  }
}