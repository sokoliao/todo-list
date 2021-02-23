using BussinessLogic.Handlers.Implementation;
using BussinessLogic.Handlers.Validation;
using DataAccess.Abstraction;
using Moq;
using Xunit;

namespace BussinessLogic.Handlers.Tests
{
    public class DeleteCommandHandlerTests
    {
        [Fact]
        public async void ShouldCreateNewTaskWhenValid()
        {
            // Arrange

            var validator = new Mock<IDeleteTodoTaskValidator>();
            var repository = new Mock<ITodoTaskEntityRepository>();

            validator.Setup(m => m.ValidateAndThrow(It.IsAny<TodoTaskEntity>()));

            var task = new TodoTaskEntity();
            repository.Setup(m => m.GetById(It.IsAny<string>())).ReturnsAsync(task);
            repository.Setup(m => m.DeleteAsync(It.IsAny<TodoTaskEntity>()));

            var handler = new DeleteCommandHandler(
                repository.Object, validator.Object);

            //  Act

            await handler.HandleAsync("afab15bc-d16b-49b8-a072-6bc1bc0d5156");

            // Assert

            validator.Verify(m => m.ValidateAndThrow(It.IsAny<TodoTaskEntity>()), Times.Once);
            repository.Verify(
                m => m.GetById(It.Is<string>(p => p == "afab15bc-d16b-49b8-a072-6bc1bc0d5156")), Times.Once);
            repository.Verify(m => m.DeleteAsync(It.Is<TodoTaskEntity>(p => p == task)), Times.Once);
        }
    }
}