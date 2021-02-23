using BussinessLogic.Handlers.Validation;
using DataAccess.Abstraction;
using Shared.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BussinessLogic.Handlers.Tests
{
    public class DeleteTaskValidatorTests
    {
        [Fact]
        public async Task ShouldInvalidateWhenNotInCompleteStatus()
        {
            // Arrange

            var validator = new DeleteTodoTaskValidator();

            // Act & Assert

            await Assert.ThrowsAsync<TodoListValidationException>(() => 
                validator.ValidateAndThrow(
                    new TodoTaskEntity
                    {
                        Id = "afab15bc-d16b-49b8-a072-6bc1bc0d5156",
                        Name = "Make a toast",
                        Priority = 1,
                        Status = TodoTaskEntityStatus.InProgress
                    }));
        }

        [Fact]
        public void ShouldValidateWhenInCompleteStatus()
        {
            // Arrange

            var validator = new DeleteTodoTaskValidator();

            // Act & Assert

            validator.ValidateAndThrow(
                new TodoTaskEntity
                {
                    Id = "afab15bc-d16b-49b8-a072-6bc1bc0d5156",
                    Name = "Make a toast",
                    Priority = 1,
                    Status = TodoTaskEntityStatus.Completed
                });
        }
    }
}