using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.Abstraction.Model;
using BussinessLogic.Handlers.Implementation;
using BussinessLogic.Handlers.Validation;
using DataAccess.Abstraction;
using DataAccess.Inmemory.Stores;
using Moq;
using Xunit;

namespace BussinessLogic.Handlers.Tests
{
    public class CreateTodoTaskCommandHandlerTests
    {
        [Fact]
        public async void ShouldCreateNewTaskWhenValid()
        {
            // Arrange

            var validator = new Mock<ICreateTodoTaskValidator>();
            var repository = new Mock<ITodoTaskEntityRepository>();

            validator.Setup(m => m.ValidateAndThrow(It.IsAny<CreateTodoTaskCommand>()));

            repository
                .Setup(m => m.CreateAsync(It.IsAny<TodoTaskEntity>()))
                .ReturnsAsync("afab15bc-d16b-49b8-a072-6bc1bc0d5156");

            var handler = new CreateTodoTaskCommandHandler(
                repository.Object, validator.Object);

            //  Act

            var result = await handler.HandleAsync(
              new CreateTodoTaskCommand
              {
                  Name = "Buy an avocado",
                  Priority = 1,
                  Status = TodoTaskStatus.InProgress
              });

            // Assert

            validator.Verify(m => m.ValidateAndThrow(It.IsAny<CreateTodoTaskCommand>()), Times.Once);
            repository.Verify(m => m.CreateAsync(It.IsAny<TodoTaskEntity>()), Times.Once);
        }
    }
}