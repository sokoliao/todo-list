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
    public class DeleteTodoTaskValidator : IDeleteTodoTaskValidator
    {
        public Task ValidateAndThrow(TodoTaskEntity context)
        {
            if (context == null)
            {
                throw new TodoListValidationException($"Task was not found");
            }

            if (string.IsNullOrEmpty(context.Id))
            {
                throw new TodoListValidationException($"{nameof(CreateTodoTaskCommand.Name)} can't be empty");
            }

            if (context.Status != TodoTaskEntityStatus.Completed)
            {
                throw new TodoListValidationException(
                    $"Can't delete task not in {TodoTaskStatus.Completed} {nameof(CreateTodoTaskCommand.Status)}");
            }

            return Task.CompletedTask;
        }
    }

    public interface IDeleteTodoTaskValidator
    {
        Task ValidateAndThrow(TodoTaskEntity context);
    }
}
