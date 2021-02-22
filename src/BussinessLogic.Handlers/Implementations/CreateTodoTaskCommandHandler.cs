using BussinessLogic.Abstraction.Handlers;
using BussinessLogic.Abstraction.Model;
using BussinessLogic.Handlers.Validation;
using DataAccess.Abstraction;
using System.Threading.Tasks;

namespace BussinessLogic.Handlers.Implementation
{
    public class CreateTodoTaskCommandHandler : ICreateTodoTaskCommandHandler
    {
        private readonly ITodoTaskEntityRepository repository;
        private readonly ICreateTodoTaskValidator validator;

        public CreateTodoTaskCommandHandler(
            ITodoTaskEntityRepository repository,
            ICreateTodoTaskValidator validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        public async Task<string> HandleAsync(CreateTodoTaskCommand command)
        {
            await validator.ValidateAndThrow(command);
            return await repository.CreateAsync(command.ToEntity());
        }
    }
}