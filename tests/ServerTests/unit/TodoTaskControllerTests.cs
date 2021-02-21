using System;
using System.Linq;
using System.Threading.Tasks;
using BxTestTask.Controllers;
using BxTestTask.Handlers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ServerTests.unit
{
  public class TodoTaskControllerTests
  {
    [Fact]
    public async Task ShouldGetAllTasksWhenThereAreSome()
    {
      // Arrange

      var logger = new Mock<ILogger<TodoTaskController>>();
      var create = new Mock<ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult>>();
      var get = new Mock<IQueryHandler<GetAllQuery, GetAllQueryResult>>();
      var delete = new Mock<ICommandHandler<DeleteCommand>>();
      var update = new Mock<ICommandHandler<UpdateCommand>>();

      get.Setup(m => m.HandleAsync(It.IsAny<GetAllQuery>()))
        .Returns(Task.FromResult(new GetAllQueryResult { Tasks = tasks }));
      
      var controller = new TodoTaskController(
        logger.Object,
        create.Object,
        get.Object,
        delete.Object,
        update.Object
      );

      // Act

      var result = await controller.GetAllAsync();

      // Assert

      Assert.NotNull(result);
      Assert.Equal(2, result.Count());
      Assert.Equal(tasks[0].Id, result.ElementAt(0).Id);
      Assert.Equal(tasks[1].Id, result.ElementAt(1).Id);
      get.Verify(m => m.HandleAsync(It.IsAny<GetAllQuery>()), Times.Once);
    }

    [Fact]
    public async Task ShouldGetAllTasksWhenThereIsOne()
    {
      // Arrange

      var logger = new Mock<ILogger<TodoTaskController>>();
      var create = new Mock<ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult>>();
      var get = new Mock<IQueryHandler<GetAllQuery, GetAllQueryResult>>();
      var delete = new Mock<ICommandHandler<DeleteCommand>>();
      var update = new Mock<ICommandHandler<UpdateCommand>>();

      get.Setup(m => m.HandleAsync(It.IsAny<GetAllQuery>()))
        .Returns(Task.FromResult(new GetAllQueryResult { Tasks = tasks.Take(1).ToArray() }));
      
      var controller = new TodoTaskController(
        logger.Object,
        create.Object,
        get.Object,
        delete.Object,
        update.Object
      );

      // Act

      var result = await controller.GetAllAsync();

      // Assert

      Assert.NotNull(result);
      Assert.Single(result);
      Assert.Equal(tasks[0].Id, result.ElementAt(0).Id);
      get.Verify(m => m.HandleAsync(It.IsAny<GetAllQuery>()), Times.Once);
    }

    [Fact]
    public async Task ShouldGetAllTasksWhenThereIsNone()
    {
      // Arrange

      var logger = new Mock<ILogger<TodoTaskController>>();
      var create = new Mock<ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult>>();
      var get = new Mock<IQueryHandler<GetAllQuery, GetAllQueryResult>>();
      var delete = new Mock<ICommandHandler<DeleteCommand>>();
      var update = new Mock<ICommandHandler<UpdateCommand>>();

      get.Setup(m => m.HandleAsync(It.IsAny<GetAllQuery>()))
        .Returns(Task.FromResult(new GetAllQueryResult { Tasks = new TodoTask[0] }));
      
      var controller = new TodoTaskController(
        logger.Object,
        create.Object,
        get.Object,
        delete.Object,
        update.Object
      );

      // Act

      var result = await controller.GetAllAsync();

      // Assert

      Assert.NotNull(result);
      Assert.Empty(result);
      get.Verify(m => m.HandleAsync(It.IsAny<GetAllQuery>()), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowWhenGetAllHandlerThrows()
    {
      // Arrange

      var logger = new Mock<ILogger<TodoTaskController>>();
      var create = new Mock<ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult>>();
      var get = new Mock<IQueryHandler<GetAllQuery, GetAllQueryResult>>();
      var delete = new Mock<ICommandHandler<DeleteCommand>>();
      var update = new Mock<ICommandHandler<UpdateCommand>>();

      get.Setup(m => m.HandleAsync(It.IsAny<GetAllQuery>()))
        .Throws(new TestException());
      
      var controller = new TodoTaskController(
        logger.Object,
        create.Object,
        get.Object,
        delete.Object,
        update.Object
      );

      // Act & Assert

      var result = await Assert.ThrowsAsync<TestException>(() => controller.GetAllAsync());
    }

    [Fact]
    public async Task ShouldCreateTask()
    {
      // Arrange

      var logger = new Mock<ILogger<TodoTaskController>>();
      var create = new Mock<ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult>>();
      var get = new Mock<IQueryHandler<GetAllQuery, GetAllQueryResult>>();
      var delete = new Mock<ICommandHandler<DeleteCommand>>();
      var update = new Mock<ICommandHandler<UpdateCommand>>();

      var newId = Guid.Parse("94b91e96-1b5a-417a-a0fa-24f9cda616f4");
      create
        .Setup(m => m.HandleAsync(It.IsAny<CreateTodoTaskCommand>()))
        .Returns(Task.FromResult(new CreateTodoTaskResult { Id = newId }));
      
      var controller = new TodoTaskController(
        logger.Object,
        create.Object,
        get.Object,
        delete.Object,
        update.Object
      );

      // Act

      var result = await controller.CreateAsync(
        new CreateTodoTaskModel
        {
          Name = "Buy an avocado",
          Priority = 1,
          Status = TodoTaskModelStatus.InProgress
        });

      // Assert

      Assert.NotNull(result);
      Assert.Equal(newId, result.Id);
      create.Verify(
        m => m.HandleAsync(It.Is<CreateTodoTaskCommand>(p =>
             p.Name == "Buy an avocado"
          && p.Priority == 1
          && p.Status == TodoTaskStatus.InProgress)), 
        Times.Once);
      create.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowWhenCreateHandlerThrows()
    {
      // Arrange

      var logger = new Mock<ILogger<TodoTaskController>>();
      var create = new Mock<ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult>>();
      var get = new Mock<IQueryHandler<GetAllQuery, GetAllQueryResult>>();
      var delete = new Mock<ICommandHandler<DeleteCommand>>();
      var update = new Mock<ICommandHandler<UpdateCommand>>();

      create
        .Setup(m => m.HandleAsync(It.IsAny<CreateTodoTaskCommand>()))
        .Throws(new TestException());
      
      var controller = new TodoTaskController(
        logger.Object,
        create.Object,
        get.Object,
        delete.Object,
        update.Object
      );

      // Act

      await Assert.ThrowsAsync<TestException>(() => 
        controller.CreateAsync(
          new CreateTodoTaskModel
          {
            Name = "Buy an avocado",
            Priority = 1,
            Status = TodoTaskModelStatus.InProgress
          }));

      // Assert

      create.Verify(
        m => m.HandleAsync(It.Is<CreateTodoTaskCommand>(p =>
             p.Name == "Buy an avocado"
          && p.Priority == 1
          && p.Status == TodoTaskStatus.InProgress)), 
        Times.Once);
      create.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldUpdateTask()
    {
      // Arrange

      var logger = new Mock<ILogger<TodoTaskController>>();
      var create = new Mock<ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult>>();
      var get = new Mock<IQueryHandler<GetAllQuery, GetAllQueryResult>>();
      var delete = new Mock<ICommandHandler<DeleteCommand>>();
      var update = new Mock<ICommandHandler<UpdateCommand>>();

      update
        .Setup(m => m.HandleAsync(It.IsAny<UpdateCommand>()))
        .Returns(Task.CompletedTask);
      
      var controller = new TodoTaskController(
        logger.Object,
        create.Object,
        get.Object,
        delete.Object,
        update.Object
      );

      // Act

      await controller.UpdateAsync(
        tasks[0].Id,
        new UpdateTodoTaskModel
        {
          Name = "Buy an avocado",
          Priority = 1,
          Status = TodoTaskModelStatus.InProgress
        });

      // Assert

      update.Verify(
        m => m.HandleAsync(It.Is<UpdateCommand>(p =>
             p.Id == tasks[0].Id
          && p.Name == "Buy an avocado"
          && p.Priority == 1
          && p.Status == TodoTaskStatus.InProgress)), 
        Times.Once);
      update.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowWhenUpdateHandlerThrows()
    {
      // Arrange

      var logger = new Mock<ILogger<TodoTaskController>>();
      var create = new Mock<ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult>>();
      var get = new Mock<IQueryHandler<GetAllQuery, GetAllQueryResult>>();
      var delete = new Mock<ICommandHandler<DeleteCommand>>();
      var update = new Mock<ICommandHandler<UpdateCommand>>();

      update
        .Setup(m => m.HandleAsync(It.IsAny<UpdateCommand>()))
        .Throws(new TestException());
      
      var controller = new TodoTaskController(
        logger.Object,
        create.Object,
        get.Object,
        delete.Object,
        update.Object
      );

      // Act

      await Assert.ThrowsAsync<TestException>(() => 
        controller.UpdateAsync(
          tasks[0].Id,
          new UpdateTodoTaskModel
          {
            Name = "Buy an avocado",
            Priority = 1,
            Status = TodoTaskModelStatus.InProgress
          }));

      // Assert

      update.Verify(
        m => m.HandleAsync(It.Is<UpdateCommand>(p =>
             p.Id == tasks[0].Id
          && p.Name == "Buy an avocado"
          && p.Priority == 1
          && p.Status == TodoTaskStatus.InProgress)), 
        Times.Once);
      update.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldDeleteTask()
    {
      // Arrange

      var logger = new Mock<ILogger<TodoTaskController>>();
      var create = new Mock<ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult>>();
      var get = new Mock<IQueryHandler<GetAllQuery, GetAllQueryResult>>();
      var delete = new Mock<ICommandHandler<DeleteCommand>>();
      var update = new Mock<ICommandHandler<UpdateCommand>>();

      delete
        .Setup(m => m.HandleAsync(It.IsAny<DeleteCommand>()))
        .Returns(Task.CompletedTask);
      
      var controller = new TodoTaskController(
        logger.Object,
        create.Object,
        get.Object,
        delete.Object,
        update.Object
      );

      // Act

      await controller.Delete(tasks[0].Id);

      // Assert

      delete.Verify(
        m => m.HandleAsync(It.Is<DeleteCommand>(p => p.Id == tasks[0].Id)), 
        Times.Once);
      delete.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowWhenDeleteHandlerThrows()
    {
      // Arrange

      var logger = new Mock<ILogger<TodoTaskController>>();
      var create = new Mock<ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult>>();
      var get = new Mock<IQueryHandler<GetAllQuery, GetAllQueryResult>>();
      var delete = new Mock<ICommandHandler<DeleteCommand>>();
      var update = new Mock<ICommandHandler<UpdateCommand>>();

      delete
        .Setup(m => m.HandleAsync(It.IsAny<DeleteCommand>()))
        .Throws(new TestException());
      
      var controller = new TodoTaskController(
        logger.Object,
        create.Object,
        get.Object,
        delete.Object,
        update.Object
      );

      // Act

      await Assert.ThrowsAsync<TestException>(() => controller.Delete(tasks[0].Id));

      // Assert

      delete.Verify(
        m => m.HandleAsync(It.Is<DeleteCommand>(p => p.Id == tasks[0].Id)), 
        Times.Once);
      delete.VerifyNoOtherCalls();
    }

    private class TestException : Exception { }

    private readonly TodoTask[] tasks = new TodoTask[2] 
    {
      new TodoTask
      {
        Id = Guid.NewGuid(),
        Name = "Make a toast",
        Priority = 1,
        Status = TodoTaskStatus.NotStarted
      },
      new TodoTask
      {
        Id = Guid.NewGuid(),
        Name = "Eat a toast",
        Priority = 1,
        Status = TodoTaskStatus.NotStarted
      }
    };
  }
}