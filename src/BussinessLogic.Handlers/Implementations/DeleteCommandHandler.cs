using BussinessLogic.Abstraction.Handlers;
using BussinessLogic.Abstraction.Model;
using BussinessLogic.Handlers.Validation;
using DataAccess.Abstraction;
using System.Threading.Tasks;

namespace BussinessLogic.Handlers.Implementation
{
    public class DeleteCommandHandler : IDeleteCommandHandler
    {
        private readonly ITodoTaskEntityRepository repository;
        private readonly IDeleteTodoTaskValidator validator;

        public DeleteCommandHandler(
            ITodoTaskEntityRepository repository,
            IDeleteTodoTaskValidator validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        public async Task HandleAsync(TodoTask task)
        {
            await validator.ValidateAndThrow(task);
            await repository.DeleteAsync(task.ToEntity());
        }
    }
}