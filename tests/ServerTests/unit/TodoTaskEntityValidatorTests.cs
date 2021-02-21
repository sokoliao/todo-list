using System;
using BxTestTask.Stores;
using BxTestTask.Validators;
using Xunit;

namespace ServerTests.unit
{
  public class TodoTaskEntityValidatorTests
  {
    [Fact]
    public void ShouldInvalidateWhenEmptyId()
    {
      // Arrange

      var validator = new TodoTaskEntityValidator();

      // Act

      var result = validator.Validate(new TodoTaskEntity
      {
        Id = Guid.Empty,
        Name = "Make a toast",
        Priority = 1,
        Status = TodoTaskEntityStatus.InProgress
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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldInvalidateWhenBadName(string name)
    {
      // Arrange

      var validator = new TodoTaskEntityValidator();

      // Act

      var result = validator.Validate(new TodoTaskEntity
      {
        Id = Guid.NewGuid(),
        Name = name,
        Priority = 1,
        Status = TodoTaskEntityStatus.InProgress
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

    [Theory]
    [InlineData(-2147483648)]
    [InlineData(-7)]
    [InlineData(0)]
    public void ShouldInvalidateWhenBadPriority(int priority)
    {
      // Arrange

      var validator = new TodoTaskEntityValidator();

      // Act

      var result = validator.Validate(new TodoTaskEntity
      {
        Id = Guid.NewGuid(),
        Name = "Make a toast",
        Priority = priority,
        Status = TodoTaskEntityStatus.InProgress
      });

      // Assert

      Assert.False(result.IsValid);
      Assert.Collection(result.Errors,
        error => 
        {
          Assert.Equal(Code.VALIDATION, error.ErrorCode);
          Assert.Equal(Field.PRIORITY, error.PropertyName);
        });
    }

    [Fact]
    public void ShouldValidate()
    {
      // Arrange

      var validator = new TodoTaskEntityValidator();

      // Act

      var result = validator.Validate(new TodoTaskEntity
      {
        Id = Guid.NewGuid(),
        Name = "Make a toast",
        Priority = 1,
        Status = TodoTaskEntityStatus.InProgress
      });

      // Assert

      Assert.True(result.IsValid);
    }
  }
}