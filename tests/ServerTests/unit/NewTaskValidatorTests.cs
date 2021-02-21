using System;
using BxTestTask.Stores;
using BxTestTask.Validators;
using Xunit;

namespace ServerTests.unit
{
  public class NewTaskValidatorTests
  {
    [Fact]
    public void ShouldInvalidateWhenNonUniqueId()
    {
      // Arrange

      var validator = new NewTaskValidator();

      // Act

      var result = validator.Validate(new NewTaskValidationContext
      {
        Tasks = NewTaskValidatorTests.tasks,
        NewTask = new TodoTaskEntity
        {
          Id = NewTaskValidatorTests.tasks[0].Id,
          Name = "Buy an avocado",
          Priority = 1,
          Status = TodoTaskEntityStatus.InProgress
        }
      });

      // Assert

      Assert.False(result.IsValid);
      Assert.Collection(result.Errors,
        error => 
        {
          Assert.Equal(Code.VALIDATION, error.ErrorCode);
          Assert.Equal(Field.ID, error.PropertyName);
        });
    }

    [Fact]
    public void ShouldInvalidateWhenNonUniqueName()
    {
      // Arrange

      var validator = new NewTaskValidator();

      // Act

      var result = validator.Validate(new NewTaskValidationContext
      {
        Tasks = NewTaskValidatorTests.tasks,
        NewTask = new TodoTaskEntity
        {
          Id = Guid.NewGuid(),
          Name = NewTaskValidatorTests.tasks[0].Name,
          Priority = 1,
          Status = TodoTaskEntityStatus.InProgress
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

      var validator = new NewTaskValidator();

      // Act

      var result = validator.Validate(new NewTaskValidationContext
      {
        Tasks = NewTaskValidatorTests.tasks,
        NewTask = new TodoTaskEntity
        {
          Id = Guid.NewGuid(),
          Name = "Buy an avocado",
          Priority = 1,
          Status = TodoTaskEntityStatus.InProgress
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