using System.Linq;
using System.Threading.Tasks;
using BxTestTask.Stores;
using BxTestTask.Validators;
using FluentValidation;

namespace BxTestTask.Handlers
{
  public class DeleteCommandHandler 
    : ICommandHandler<DeleteCommand>
  {
    private readonly ITodoTaskStore store;
    private readonly IValidator<DeleteTaskValidationContext> validator;

    public DeleteCommandHandler(
      ITodoTaskStore store,
      IValidator<DeleteTaskValidationContext> validator)
    {
      this.store = store;
      this.validator = validator;
    }

    public async Task HandleAsync(DeleteCommand command)
    {
      await store.Set(current => 
      {
        var context = new DeleteTaskValidationContext
        {
          Id = command.Id,
          TaskToDelete = current.FirstOrDefault(task => task.Id == command.Id)
        };

        validator.ValidateAndThrow(context);

        return current.Where(task => task.Id != command.Id);
      });
    }
  }
}