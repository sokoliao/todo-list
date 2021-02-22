using BussinessLogic.Abstraction.Handlers;
using BussinessLogic.Abstraction.Model;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Handlers.Validation
{
    public class CreateTodoTaskValidator : ICreateTodoTaskValidator
    {
        public Task ValidateAndThrow(CreateTodoTaskCommand context)
        {
            if (string.IsNullOrWhiteSpace(context.Name))
            {
                throw new TodoListValidationException($"{nameof(CreateTodoTaskCommand.Name)} can't be empty");
            }

            if (context.Priority <= 0)
            {
                throw new TodoListValidationException($"{nameof(CreateTodoTaskCommand.Priority)} must be positive");
            }

            return Task.CompletedTask;
        }
    }

    public interface ICreateTodoTaskValidator
    {
        Task ValidateAndThrow(CreateTodoTaskCommand context);
    }
}
