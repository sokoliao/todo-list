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
  public class CreateTodoTaskCommandHandlerTests
  {

    [Fact]
    public async void ShouldAppendNewTaskWhenValid()
    {
      // Arrange

      var validator = new AlwaysValidValidator();
      var store = new Mock<ITodoTaskStore>();

      var tasks = CreateTodoTaskCommandHandlerTests.tasks.ToArray();
      store
        .Setup(m => m.Set(It.IsAny<Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>>>()))
        .Callback((Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>> setter) => tasks = setter(tasks).ToArray())
        .Returns(Task.CompletedTask);

      var handler = new CreateTodoTaskCommandHandler(store.Object, validator);

      //  Act

      var result = await handler.HandleAsync(
        new CreateTodoTaskCommand
        {
          Name = "Buy an avocado",
          Priority = 1,
          Status = TodoTaskStatus.InProgress
        });

      // Assert

      // TODO inject Id generator
      // Assert.Equal(, result.Id);
      Assert.Equal(3, tasks.Length);
      Assert.Equal("Buy an avocado", tasks[2].Name);
      Assert.Equal(1, tasks[2].Priority);
      Assert.Equal(TodoTaskEntityStatus.InProgress, tasks[2].Status);
    }

    [Fact]
    public async void ShouldCreateNewTaskWhenValidAndStoreIsEmpty()
    {
      // Arrange

      var validator = new AlwaysValidValidator();
      var store = new Mock<ITodoTaskStore>();

      var tasks = new TodoTaskEntity[0];
      store
        .Setup(m => m.Set(It.IsAny<Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>>>()))
        .Callback((Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>> setter) => tasks = setter(tasks).ToArray())
        .Returns(Task.CompletedTask);

      var handler = new CreateTodoTaskCommandHandler(store.Object, validator);

      //  Act

      var result = await handler.HandleAsync(
        new CreateTodoTaskCommand
        {
          Name = "Buy an avocado",
          Priority = 1,
          Status = TodoTaskStatus.InProgress
        });

      // Assert

      // TODO inject Id generator
      // Assert.Equal(, result.Id);
      Assert.Single(tasks);
      Assert.Equal("Buy an avocado", tasks[0].Name);
      Assert.Equal(1, tasks[0].Priority);
      Assert.Equal(TodoTaskEntityStatus.InProgress, tasks[0].Status);
    }

    [Fact]
    public async void ShouldThrowWhenInvalid()
    {
      // Arrange

      var validator = new AlwaysInvalidValidator();
      var store = new Mock<ITodoTaskStore>();

      var tasks = new TodoTaskEntity[0];
      store
        .Setup(m => m.Set(It.IsAny<Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>>>()))
        .Callback((Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>> setter) => tasks = setter(tasks).ToArray())
        .Returns(Task.CompletedTask);

      var handler = new CreateTodoTaskCommandHandler(store.Object, validator);

      //  Act

      await Assert.ThrowsAsync<ValidationException>(() => 
        handler.HandleAsync(
          new CreateTodoTaskCommand
          {
            Name = "Buy an avocado",
            Priority = 1,
            Status = TodoTaskStatus.InProgress
          }));

      // Assert

      Assert.Empty(tasks);
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

      var handler = new CreateTodoTaskCommandHandler(store.Object, validator);

      //  Act & Assert

      await Assert.ThrowsAsync<TestException>(() => 
        handler.HandleAsync(
          new CreateTodoTaskCommand
          {
            Name = "Buy an avocado",
            Priority = 1,
            Status = TodoTaskStatus.InProgress
          }));
    }

    private class AlwaysValidValidator : AbstractValidator<NewTaskValidationContext> { }

    private class AlwaysInvalidValidator : AbstractValidator<NewTaskValidationContext> 
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