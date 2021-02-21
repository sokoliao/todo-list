using System;
using System.Linq;
using System.Threading.Tasks;
using BxTestTask.Handlers;
using BxTestTask.Stores;
using Moq;
using Xunit;

namespace ServerTests.unit
{
  public class GetAllQueryHandlerTests
  {
    [Fact]
    public async Task ShouldGetTasksWhenThereAreSome()
    {
      // Arrange

      var store = new Mock<ITodoTaskStore>();

      store
        .Setup(m => m.Get())
        .Returns(Task.FromResult(GetAllQueryHandlerTests.tasks.AsEnumerable()));

      var handler = new GetAllQueryHandler(store.Object);

      //  Act

      var result = await handler.HandleAsync(new GetAllQuery());

      // Assert

      Assert.NotNull(result);
      Assert.NotNull(result.Tasks);
      Assert.Collection(
        result.Tasks, 
        first => Assert.Equal(GetAllQueryHandlerTests.tasks[0].Id, first.Id),
        second => Assert.Equal(GetAllQueryHandlerTests.tasks[1].Id, second.Id));
    }

    [Fact]
    public async Task ShouldGetNothingWhenThereIsNoTasks()
    {
      // Arrange

      var store = new Mock<ITodoTaskStore>();

      store
        .Setup(m => m.Get())
        .Returns(Task.FromResult(new TodoTaskEntity[0].AsEnumerable()));

      var handler = new GetAllQueryHandler(store.Object);

      //  Act

      var result = await handler.HandleAsync(new GetAllQuery());

      // Assert

      Assert.NotNull(result);
      Assert.NotNull(result.Tasks);
      Assert.Empty(result.Tasks);
    }

    [Fact]
    public async Task ShouldThrowWhenStoreThrows()
    {
      // Arrange

      var store = new Mock<ITodoTaskStore>();

      store
        .Setup(m => m.Get())
        .Throws(new TestException());

      var handler = new GetAllQueryHandler(store.Object);

      //  Act & Assert

      await Assert.ThrowsAnyAsync<TestException>(() => handler.HandleAsync(new GetAllQuery()));
    }

    private class TestException : Exception { }

    private static readonly TodoTaskEntity[] tasks = new TodoTaskEntity[2] 
    {
      new TodoTaskEntity
      {
        Id = Guid.NewGuid(),
        Name = "Make a toast",
        Priority = 1,
        Status = TodoTaskEntityStatus.NotStarted
      },
      new TodoTaskEntity
      {
        Id = Guid.NewGuid(),
        Name = "Eat a toast",
        Priority = 1,
        Status = TodoTaskEntityStatus.NotStarted
      }
    };
  }
}