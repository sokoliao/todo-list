using System;
using BxTestTask.Controllers;
using BxTestTask.Handlers;
using Xunit;

namespace ServerTests.unit
{
  public class ControllerModelMapperTets
  {
    [Theory]
    [InlineData("Make a toast", "Make a toast")]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void ShouldMapCreateTodoTaskModelName(string model, string expected)
    {
      // Arrange & Act

      var command = new CreateTodoTaskModel
      {
        Name = model,
        Priority = 1,
        Status = TodoTaskModelStatus.NotStarted
      }.ToCommand();

      // Assert

      Assert.Equal(expected, command.Name);
    }

    [Theory]
    [InlineData(-2147483648, -2147483648)]
    [InlineData(-7, -7)]
    [InlineData(0, 0)]
    [InlineData(7, 7)]
    [InlineData(2147483647, 2147483647)]
    public void ShouldMapCreateTodoTaskModelPriority(int model, int expected)
    {
      // Arrange & Act

      var command = new CreateTodoTaskModel
      {
        Name = "Make a toast",
        Priority = model,
        Status = TodoTaskModelStatus.NotStarted
      }.ToCommand();

      // Assert

      Assert.Equal(expected, command.Priority);
    }

    [Theory]
    [InlineData(TodoTaskModelStatus.NotStarted, TodoTaskStatus.NotStarted)]
    [InlineData(TodoTaskModelStatus.InProgress, TodoTaskStatus.InProgress)]
    [InlineData(TodoTaskModelStatus.Completed, TodoTaskStatus.Completed)]
    public void ShouldMapCreateTodoTaskModelStatus(TodoTaskModelStatus model, TodoTaskStatus expected)
    {
      // Arrange & Act

      var command = new CreateTodoTaskModel
      {
        Name = "Make a toast",
        Priority = 3,
        Status = model
      }.ToCommand();

      // Assert

      Assert.Equal(expected, command.Status);
    }

    [Theory]
    [InlineData("94b91e96-1b5a-417a-a0fa-24f9cda616f4", "94b91e96-1b5a-417a-a0fa-24f9cda616f4")]
    [InlineData("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
    public void ShouldMapTodoTaskId(string model, string expected)
    {
      // Arrange & Act

      var result = new TodoTask
      {
        Id = Guid.Parse(model),
        Name = "Make a toast",
        Priority = 1,
        Status = TodoTaskStatus.NotStarted
      }.ToModel();

      // Assert

      Assert.Equal(Guid.Parse(expected), result.Id);
    }

    [Theory]
    [InlineData("Make a toast", "Make a toast")]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void ShouldMapTodoTaskName(string model, string expected)
    {
      // Arrange & Act

      var result = new TodoTask
      {
        Id = Guid.NewGuid(),
        Name = model,
        Priority = 1,
        Status = TodoTaskStatus.NotStarted
      }.ToModel();

      // Assert

      Assert.Equal(expected, result.Name);
    }

    [Theory]
    [InlineData(-2147483648, -2147483648)]
    [InlineData(-7, -7)]
    [InlineData(0, 0)]
    [InlineData(7, 7)]
    [InlineData(2147483647, 2147483647)]
    public void ShouldMapTodoTaskPriority(int model, int expected)
    {
      // Arrange & Act

      var result = new TodoTask
      {
        Id = Guid.NewGuid(),
        Name = "Make a toast",
        Priority = model,
        Status = TodoTaskStatus.NotStarted
      }.ToModel();

      // Assert

      Assert.Equal(expected, result.Priority);
    }

    [Theory]
    [InlineData(TodoTaskStatus.NotStarted, TodoTaskModelStatus.NotStarted)]
    [InlineData(TodoTaskStatus.InProgress, TodoTaskModelStatus.InProgress)]
    [InlineData(TodoTaskStatus.Completed, TodoTaskModelStatus.Completed)]
    public void ShouldMapTodoTaskStatus(TodoTaskStatus model, TodoTaskModelStatus expected)
    {
      // Arrange & Act

      var result = new TodoTask
      {
        Id = Guid.NewGuid(),
        Name = "Make a toast",
        Priority = 1,
        Status = model
      }.ToModel();

      // Assert

      Assert.Equal(expected, result.Status);
    }

    [Theory]
    [InlineData("94b91e96-1b5a-417a-a0fa-24f9cda616f4", "94b91e96-1b5a-417a-a0fa-24f9cda616f4")]
    [InlineData("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
    public void ShouldMapUpdateTodoTaskModelId(string model, string expected)
    {
      // Arrange & Act

      var result = new UpdateTodoTaskModel
      {
        Name =  "Make a toast",
        Priority = 1,
        Status = TodoTaskModelStatus.NotStarted
      }.ToCommand(Guid.Parse(model));

      // Assert

      Assert.Equal(Guid.Parse(expected), result.Id);
    }

    [Theory]
    [InlineData("Make a toast", "Make a toast")]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void ShouldMapUpdateTodoTaskModelName(string model, string expected)
    {
      // Arrange & Act

      var result  =  new UpdateTodoTaskModel
      {
        Name = model,
        Priority = 1,
        Status = TodoTaskModelStatus.NotStarted
      }.ToCommand(Guid.NewGuid());

      // Assert

      Assert.Equal(expected, result.Name);
    }

    [Theory]
    [InlineData(-2147483648, -2147483648)]
    [InlineData(-7, -7)]
    [InlineData(0, 0)]
    [InlineData(7, 7)]
    [InlineData(2147483647, 2147483647)]
    public void ShouldMapUpdateTodoTaskModelPriority(int model, int expected)
    {
      // Arrange & Act

      var result  =  new UpdateTodoTaskModel
      {
        Name = "Make a toast",
        Priority = model,
        Status = TodoTaskModelStatus.NotStarted
      }.ToCommand(Guid.NewGuid());

      // Assert

      Assert.Equal(expected, result.Priority);
    }

    [Theory]
    [InlineData(TodoTaskModelStatus.NotStarted, TodoTaskStatus.NotStarted)]
    [InlineData(TodoTaskModelStatus.InProgress, TodoTaskStatus.InProgress)]
    [InlineData(TodoTaskModelStatus.Completed, TodoTaskStatus.Completed)]
    public void ShouldMapUpdateTodoTaskModelStatus(TodoTaskModelStatus model, TodoTaskStatus expected)
    {
      // Arrange & Act

      var result  =  new UpdateTodoTaskModel
      {
        Name = "Make a toast",
        Priority = 1,
        Status = model
      }.ToCommand(Guid.NewGuid());

      // Assert

      Assert.Equal(expected, result.Status);
    }
  }
}