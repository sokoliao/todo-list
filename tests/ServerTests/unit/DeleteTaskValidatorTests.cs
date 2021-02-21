using System;
using BxTestTask.Handlers;
using BxTestTask.Stores;
using BxTestTask.Validators;
using Xunit;

namespace ServerTests.unit
{
  public class DeleteTaskValidatorTests
  {
    [Fact]
    public void ShouldInvalidateWhenTaskIsMissing()
    {
      // Arrange

      var validator = new DeleteTaskValidator();

      // Act

      var result = validator.Validate(new DeleteTaskValidationContext
      {
        TaskToDelete = null,
        Id = Guid.NewGuid()
      });

      // Assert

      Assert.False(result.IsValid);
      Assert.Collection(result.Errors,
        error => 
        {
          Assert.Equal(Code.NOT_FOUND, error.ErrorCode);
          Assert.Equal(Field.ID, error.PropertyName);
        });
    }

    [Theory]
    [InlineData(TodoTaskEntityStatus.NotStarted)]
    [InlineData(TodoTaskEntityStatus.InProgress)]
    public void ShouldInvalidateWhenNotCompleted(TodoTaskEntityStatus status)
    {
      // Arrange

      var validator = new DeleteTaskValidator();
      var id = Guid.NewGuid();

      // Act

      var result = validator.Validate(new DeleteTaskValidationContext
      {
        TaskToDelete = new TodoTaskEntity
        {
          Id = Guid.NewGuid(),
          Name = "Make a toast",
          Priority = 1,
          Status = status
        },
        Id = id
      });

      // Assert

      Assert.False(result.IsValid);
      Assert.Collection(result.Errors,
        error => 
        {
          Assert.Equal(Code.VALIDATION, error.ErrorCode);
          Assert.Equal(Field.STATUS, error.PropertyName);
        });
    }

    [Fact]
    public void ShouldValidate()
    {
      // Arrange

      var validator = new DeleteTaskValidator();
      var id = Guid.NewGuid();

      // Act

      var result = validator.Validate(new DeleteTaskValidationContext
      {
        TaskToDelete = new TodoTaskEntity
        {
          Id = Guid.NewGuid(),
          Name = "Make a toast",
          Priority = 1,
          Status = TodoTaskEntityStatus.Completed
        },
        Id = id
      });

      // Assert

      Assert.True(result.IsValid);
    }
  }
}