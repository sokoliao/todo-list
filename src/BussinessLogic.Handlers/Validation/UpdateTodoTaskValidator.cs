using BussinessLogic.Abstraction.Handlers;
using BussinessLogic.Abstraction.Model;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Handlers.Validation
{
    public class UpdateTodoTaskValidator : IUpdateTodoTaskValidator
    {
        public Task ValidateAndThrow(TodoTask context)
        {
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

            return Task.CompletedTask;
        }
    }

    public interface IUpdateTodoTaskValidator
    {
        Task ValidateAndThrow(TodoTask context);
    }
}
