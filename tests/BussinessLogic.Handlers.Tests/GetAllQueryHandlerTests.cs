using System;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.Abstraction.Model;
using BussinessLogic.Handlers.Implementation;
using DataAccess.Abstraction;
using DataAccess.Inmemory.Repository;
using Moq;
using Xunit;

namespace BussinessLogic.Handlers.Tests
{
    public class GetAllQueryHandlerTests
    {
        [Fact]
        public async Task ShouldGetAll()
        {
            // Arrange

            var repository = new Mock<ITodoTaskEntityRepository>();

            var tasks = new TodoTaskEntity[]
            {
                new TodoTaskEntity
                {
                    Id = "afab15bc-d16b-49b8-a072-6bc1bc0d5156",
                    Name = "Make a toast",
                    Priority = 1,
                    Status = TodoTaskEntityStatus.InProgress
                }
            };
            repository
                .Setup(m => m.GetAll())
                .ReturnsAsync(tasks);

            var handler = new GetAllQueryHandler(repository.Object);

            // Act

            var result = await handler.HandleAsync();

            // Result

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("afab15bc-d16b-49b8-a072-6bc1bc0d5156", result.ElementAt(0).Id);
            Assert.Equal("Make a toast", result.ElementAt(0).Name);
            Assert.Equal(1, result.ElementAt(0).Priority);
            Assert.Equal(TodoTaskStatus.InProgress, result.ElementAt(0).Status);
            repository.Verify(m => m.GetAll(), Times.Once);
        }
    }
}