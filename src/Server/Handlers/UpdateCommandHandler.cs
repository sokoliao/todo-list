using System;
using System.Linq;
using System.Threading.Tasks;
using BxTestTask.Stores;
using BxTestTask.Validators;
using FluentValidation;

namespace BxTestTask.Handlers
{
  public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
  {
    private readonly ITodoTaskStore store;
    private readonly IValidator<UpdateTaskValidationContext> validator;

    public UpdateCommandHandler(
      ITodoTaskStore store,
      IValidator<UpdateTaskValidationContext> validator)
    {
      this.store = store;
      this.validator = validator;
    }

    public async Task HandleAsync(UpdateCommand command)
    {
      var updatedTask = command.ToEntity();

      await store.Set(tasks => 
      {
        var context = new UpdateTaskValidationContext
        {
          PristineTask = tasks
            .Where(task => task.Id == updatedTask.Id)
            .SingleOrDefault(),
          Tasks = tasks
            .Where(task => task.Id != updatedTask.Id),
          UpdatedTask = updatedTask
        };

        validator.ValidateAndThrow(context);

        return context.Tasks.Append(context.UpdatedTask);
      });
    }
  }
}