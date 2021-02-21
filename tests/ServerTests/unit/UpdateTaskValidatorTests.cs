using System;
using BxTestTask.Stores;
using BxTestTask.Validators;
using Xunit;

namespace ServerTests.unit
{
  public class UpdateTaskValidatorTests
  {
    [Fact]
    public void ShouldInvalidateWhenTaskIsMissing()
    {
      // Arrange

      var validator = new UpdateTaskValidator();

      // Act

      var result = validator.Validate(new UpdateTaskValidationContext
      {
        PristineTask = null,
        Tasks = UpdateTaskValidatorTests.tasks,
        UpdatedTask = new TodoTaskEntity
        {
          Id = Guid.NewGuid(),
          Name = "Buy an avocado",
          Priority = 1,
          Status = TodoTaskEntityStatus.Completed
        }
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

    [Fact]
    public void ShouldInvalidateWhenNonUniqueName()
    {
      // Arrange

      var validator = new UpdateTaskValidator();

      // Act

      var result = validator.Validate(new UpdateTaskValidationContext
      {
        PristineTask = UpdateTaskValidatorTests.tasks[0],
        Tasks = UpdateTaskValidatorTests.tasks,
        UpdatedTask = new TodoTaskEntity
        {
          Id = UpdateTaskValidatorTests.tasks[0].Id,
          Name = UpdateTaskValidatorTests.tasks[1].Name,
          Priority = 1,
          Status = TodoTaskEntityStatus.Completed
        }
      });

      // Assert

      Assert.False(result.IsValid);
      Assert.Collection(result.Errors,
        error =>
        {
          Assert.Equal(Code.VALIDATION, error.ErrorCode);
          Assert.Equal(Field.NAME, error.PropertyName);
        });
    }

    [Fact]
    public void ShouldValidate()
    {
      // Arrange

      var validator = new UpdateTaskValidator();

      // Act

      var result = validator.Validate(new UpdateTaskValidationContext
      {
        PristineTask = UpdateTaskValidatorTests.tasks[0],
        Tasks = UpdateTaskValidatorTests.tasks,
        UpdatedTask = new TodoTaskEntity
        {
          Id = UpdateTaskValidatorTests.tasks[0].Id,
          Name = "Buy an avocado",
          Priority = 1,
          Status = TodoTaskEntityStatus.Completed
        }
      });

      // Assert

      Assert.True(result.IsValid);
    }

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