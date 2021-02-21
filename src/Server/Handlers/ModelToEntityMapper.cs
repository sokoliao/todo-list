using System;
using BxTestTask.Stores;

namespace BxTestTask.Handlers
{
  public static class ModelToEntityMapper
  {
    public static TodoTaskEntity ToEntity(this CreateTodoTaskCommand command, Guid id)
    {
      return new TodoTaskEntity
      {
        Id = id,
        Name = command.Name,
        Priority = command.Priority,
        Status = Enum.Parse<TodoTaskEntityStatus>(command.Status.ToString()) 
      };
    }

    public static TodoTaskEntity ToEntity(this UpdateCommand command)
    {
      return new TodoTaskEntity
      {
        Id = command.Id,
        Name = command.Name,
        Priority = command.Priority,
        Status = Enum.Parse<TodoTaskEntityStatus>(command.Status.ToString())
      };
    }

    public static TodoTask ToModel(this TodoTaskEntity entity)
    {
      return new TodoTask
      {
        Id = entity.Id,
        Name = entity.Name,
        Priority = entity.Priority,
        Status = Enum.Parse<TodoTaskStatus>(entity.Status.ToString()) 
      };
    }
  }
}