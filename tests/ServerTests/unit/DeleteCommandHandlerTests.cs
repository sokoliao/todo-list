using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BxTestTask.Handlers;
using BxTestTask.Stores;
using BxTestTask.Validators;
using FluentValidation;
using Moq;
using Xunit;

namespace ServerTests.unit
{
  public class DeleteCommandHandlerTests
  {
    [Fact]
    public async void ShouldDeleteWhenValid()
    {
      // Arrange

      var validator = new AlwaysValidValidator();
      var store = new Mock<ITodoTaskStore>();

      var tasks = DeleteCommandHandlerTests.tasks.ToArray();
      store
        .Setup(m => m.Set(It.IsAny<Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>>>()))
        .Callback((Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>> setter) => tasks = setter(tasks).ToArray())
        .Returns(Task.CompletedTask);

      var handler = new DeleteCommandHandler(store.Object, validator);

      //  Act

      await handler.HandleAsync(
        new DeleteCommand
        {
          Id = DeleteCommandHandlerTests.tasks[0].Id
        });

      // Assert

      // TODO inject Id generator
      Assert.Collection(tasks, task => Assert.Equal(DeleteCommandHandlerTests.tasks[1].Id, task.Id));
    }

    [Fact]
    public async void ShouldThrowWhenInvalid()
    {
      // Arrange

      var validator = new AlwaysInvalidValidator();
      var store = new Mock<ITodoTaskStore>();

      var tasks = DeleteCommandHandlerTests.tasks.ToArray();
      store
        .Setup(m => m.Set(It.IsAny<Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>>>()))
        .Callback((Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>> setter) => tasks = setter(tasks).ToArray())
        .Returns(Task.CompletedTask);

      var handler = new DeleteCommandHandler(store.Object, validator);

      //  Act & Assert

      await Assert.ThrowsAsync<ValidationException>(() => 
        handler.HandleAsync(
          new DeleteCommand { Id = DeleteCommandHandlerTests.tasks[0].Id }));

      Assert.Equal(DeleteCommandHandlerTests.tasks, tasks);
    }

    [Fact]
    public async void ShouldThrowWhenStoreThrows()
    {
      // Arrange

      var validator = new AlwaysValidValidator();
      var store = new Mock<ITodoTaskStore>();

      store
        .Setup(m => m.Set(It.IsAny<Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>>>()))
        .Throws(new TestException());

      var handler = new DeleteCommandHandler(store.Object, validator);

      //  Act & Assert

      await Assert.ThrowsAsync<TestException>(() => 
        handler.HandleAsync(
          new DeleteCommand { Id = DeleteCommandHandlerTests.tasks[0].Id }));

      Assert.Equal(DeleteCommandHandlerTests.tasks, tasks);
    }

    private class AlwaysValidValidator : AbstractValidator<DeleteTaskValidationContext> { }

    private class AlwaysInvalidValidator : AbstractValidator<DeleteTaskValidationContext> 
    { 
      public AlwaysInvalidValidator()
      {
        RuleFor(ctx => ctx).Must(ctx => false);
      }
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