using BussinessLogic.Abstraction.Handlers;
using BussinessLogic.Abstraction.Model;
using BussinessLogic.Handlers.Validation;
using DataAccess.Abstraction;
using System.Threading.Tasks;

namespace BussinessLogic.Handlers.Implementation
{
    public class UpdateCommandHandler : IUpdateCommandHandler
    {
        private readonly ITodoTaskEntityRepository repository;
        private readonly IUpdateTodoTaskValidator valdiator;

        public UpdateCommandHandler(
            ITodoTaskEntityRepository repository,
            IUpdateTodoTaskValidator valdiator)
        {
            this.repository = repository;
            this.valdiator = valdiator;
        }

        public async Task HandleAsync(TodoTask task)
        {
            var prev = await repository.GetById(task.Id);
            await valdiator.ValidateAndThrow(task);
            await repository.UpdateAsync(task.ToEntity());
        }
    }
}