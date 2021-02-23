using BussinessLogic.Abstraction.Handlers;
using BussinessLogic.Abstraction.Model;
using DataAccess.Abstraction;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Handlers.Validation
{
    public class UpdateTodoTaskValidator : IUpdateTodoTaskValidator
    {
        private readonly ITodoTaskEntityRepository repository;

        public UpdateTodoTaskValidator(ITodoTaskEntityRepository repository)
        {
            this.repository = repository;
        }

        public async Task ValidateAndThrow(TodoTask context)
        {
            var prev = await repository.GetById(context.Id);

            if (prev == null)
            {
                throw new TodoListValidationException($"Task was not found");
            }

            if (string.IsNullOrWhiteSpace(context.Id))
            {
                throw new TodoListValidationException($"{nameof(TodoTask.Id)} can't be empty");
            }

            if (string.IsNullOrWhiteSpace(context.Name))
            {
                throw new TodoListValidationException($"{nameof(TodoTask.Name)} can't be empty");
            }

            if (context.Priority <= 0)
            {
                throw new TodoListValidationException($"{nameof(TodoTask.Priority)} must be positive");
            }
        }
    }

    public interface IUpdateTodoTaskValidator
    {
        Task ValidateAndThrow(TodoTask context);
    }
}
