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
  public class UpdateCommandHandlerTests 
  {
    [Fact]
    public async void ShouldUpdateTaskWhenValid()
    {
      // Arrange

      var validator = new AlwaysValidValidator();
      var store = new Mock<ITodoTaskStore>();

      var tasks = UpdateCommandHandlerTests.tasks.ToArray();
      store
        .Setup(m => m.Set(It.IsAny<Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>>>()))
        .Callback((Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>> setter) => tasks = setter(tasks).ToArray())
        .Returns(Task.CompletedTask);

      var handler = new UpdateCommandHandler(store.Object, validator);

      //  Act

      await handler.HandleAsync(
        new UpdateCommand
        {
          Id = UpdateCommandHandlerTests.tasks[0].Id,
          Name = "Buy an avocado",
          Priority = 1,
          Status = TodoTaskStatus.InProgress
        });

      // Assert

      Assert.Collection(tasks,
        first => 
        {
          Assert.Equal(UpdateCommandHandlerTests.tasks[1].Id, first.Id);
          Assert.Equal(UpdateCommandHandlerTests.tasks[1].Name, first.Name);
        },
        second => 
        {
          Assert.Equal(UpdateCommandHandlerTests.tasks[0].Id, second.Id);
          Assert.Equal("Buy an avocado", second.Name);
        });
    }

    [Fact]
    public async void ShouldThrowWhenInvalid()
    {
      // Arrange

      var validator = new AlwaysInvalidValidator();
      var store = new Mock<ITodoTaskStore>();

      var tasks = UpdateCommandHandlerTests.tasks.ToArray();
      store
        .Setup(m => m.Set(It.IsAny<Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>>>()))
        .Callback((Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>> setter) => tasks = setter(tasks).ToArray())
        .Returns(Task.CompletedTask);

      var handler = new UpdateCommandHandler(store.Object, validator);

      //  Act

      await Assert.ThrowsAsync<ValidationException>(() => 
        handler.HandleAsync(
          new UpdateCommand
          {
            Id = tasks[0].Id,
            Name = "Buy an avocado",
            Priority = 1,
            Status = TodoTaskStatus.InProgress
          }));

      // Assert

      Assert.Collection(tasks,
        first => 
        {
          Assert.Equal(UpdateCommandHandlerTests.tasks[0].Id, first.Id);
          Assert.Equal(UpdateCommandHandlerTests.tasks[0].Name, first.Name);
        },
        second => 
        {
          Assert.Equal(UpdateCommandHandlerTests.tasks[1].Id, second.Id);
          Assert.Equal(UpdateCommandHandlerTests.tasks[1].Name, second.Name);
        });
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

      var handler = new UpdateCommandHandler(store.Object, validator);

      //  Act & Assert

      await Assert.ThrowsAsync<TestException>(() => 
        handler.HandleAsync(
          new UpdateCommand
          {
            Id = Guid.NewGuid(),
            Name = "Buy an avocado",
            Priority = 1,
            Status = TodoTaskStatus.InProgress
          }));
    }

    private class AlwaysValidValidator : AbstractValidator<UpdateTaskValidationContext> { }

    private class AlwaysInvalidValidator : AbstractValidator<UpdateTaskValidationContext> 
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