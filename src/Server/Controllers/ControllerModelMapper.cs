using System;
using BxTestTask.Handlers;

namespace BxTestTask.Controllers
{
  public static class ControllerModelMapper
  {
    public static CreateTodoTaskCommand ToCommand(this CreateTodoTaskModel model)
    {
      return new CreateTodoTaskCommand
      {
        Name = model.Name,
        Priority = model.Priority,
        Status = Enum.Parse<TodoTaskStatus>(model.Status.ToString())
      };
    }

    public static TodoTaskModel ToModel(this TodoTask task)
    {
      return new TodoTaskModel
      {
        Id = task.Id,
        Name = task.Name,
        Priority = task.Priority,
        Status = Enum.Parse<TodoTaskModelStatus>(task.Status.ToString())
      };
    }

    public static UpdateCommand ToCommand(this UpdateTodoTaskModel model, Guid id)
    {
      return new UpdateCommand 
      {
        Id = id,
        Name = model.Name,
        Priority = model.Priority,
        Status = Enum.Parse<TodoTaskStatus>(model.Status.ToString())
      };
    }
  }
}