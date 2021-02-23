using System;
using BussinessLogic.Abstraction.Model;
using BussinessLogic.Handlers.Implementation;
using DataAccess.Abstraction;
using Xunit;

namespace BussinessLogic.Handlers.Tests
{
    public class ModelToEntityMapperTests
    {
        [Theory]
        [InlineData("Make a toast", "Make a toast")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void ShouldMapCreateTodoTaskCommandName(string model, string expected)
        {
            // Arrange & Act

            var result = new CreateTodoTaskCommand
            {
                Name = model,
                Priority = 1,
                Status = TodoTaskStatus.NotStarted
            }.ToEntity();

            // Assert

            Assert.Equal(expected, result.Name);
        }

        [Theory]
        [InlineData(-2147483648, -2147483648)]
        [InlineData(-7, -7)]
        [InlineData(0, 0)]
        [InlineData(7, 7)]
        [InlineData(2147483647, 2147483647)]
        public void ShouldMapCreateTodoTaskCommandPriority(int model, int expected)
        {
            // Arrange & Act

            var result = new CreateTodoTaskCommand
            {
                Name = "Make a toast",
                Priority = model,
                Status = TodoTaskStatus.NotStarted
            }.ToEntity();

            // Assert

            Assert.Equal(expected, result.Priority);
        }

        [Theory]
        [InlineData(TodoTaskStatus.NotStarted, TodoTaskEntityStatus.NotStarted)]
        [InlineData(TodoTaskStatus.InProgress, TodoTaskEntityStatus.InProgress)]
        [InlineData(TodoTaskStatus.Completed, TodoTaskEntityStatus.Completed)]
        public void ShouldMapCreateTodoTaskCommandStatus(TodoTaskStatus model, TodoTaskEntityStatus expected)
        {
            // Arrange & Act

            var command = new CreateTodoTaskCommand
            {
                Name = "Make a toast",
                Priority = 3,
                Status = model
            }.ToEntity();

            // Assert

            Assert.Equal(expected, command.Status);
        }

        [Theory]
        [InlineData("94b91e96-1b5a-417a-a0fa-24f9cda616f4", "94b91e96-1b5a-417a-a0fa-24f9cda616f4")]
        [InlineData("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
        public void ShouldMapTodoTaskEntityId(string model, string expected)
        {
            // Arrange & Act

            var result = new TodoTaskEntity
            {
                Id = model,
                Name = "Make a toast",
                Priority = 1,
                Status = TodoTaskEntityStatus.NotStarted
            }.ToModel();

            // Assert

            Assert.Equal(expected, result.Id);
        }

        [Theory]
        [InlineData("Make a toast", "Make a toast")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void ShouldMapTodoTaskEntityName(string model, string expected)
        {
            // Arrange & Act

            var result = new TodoTaskEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = model,
                Priority = 1,
                Status = TodoTaskEntityStatus.NotStarted
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
        public void ShouldMapTodoTaskEntityPriority(int model, int expected)
        {
            // Arrange & Act

            var result = new TodoTaskEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Make a toast",
                Priority = model,
                Status = TodoTaskEntityStatus.NotStarted
            }.ToModel();

            // Assert

            Assert.Equal(expected, result.Priority);
        }

        [Theory]
        [InlineData(TodoTaskEntityStatus.NotStarted, TodoTaskStatus.NotStarted)]
        [InlineData(TodoTaskEntityStatus.InProgress, TodoTaskStatus.InProgress)]
        [InlineData(TodoTaskEntityStatus.Completed, TodoTaskStatus.Completed)]
        public void ShouldMapTodoTaskEntityStatus(TodoTaskEntityStatus model, TodoTaskStatus expected)
        {
            // Arrange & Act

            var result = new TodoTaskEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Make a toast",
                Priority = 3,
                Status = model
            }.ToModel();

            // Assert

            Assert.Equal(expected, result.Status);
        }
    }
}