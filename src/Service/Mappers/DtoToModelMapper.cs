using BussinessLogic.Abstraction.Model;
using Service.Abstraction.Model;
using System;

namespace Service.Mappers
{
    public static class DtoToModelMapper
    {
        public static CreateTodoTaskCommand ToCommand(this CreateTodoTaskDto model)
        {
            return new CreateTodoTaskCommand
            {
                Name = model.Name,
                Priority = model.Priority,
                Status = Enum.Parse<TodoTaskStatus>(model.Status.ToString())
            };
        }

        public static TodoTaskDto ToDto(this TodoTask task)
        {
            return new TodoTaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Priority = task.Priority,
                Status = Enum.Parse<TodoTaskDtoStatus>(task.Status.ToString())
            };
        }

        public static TodoTask ToModel(this TodoTaskDto model)
        {
            return new TodoTask
            {
                Id = model.Id,
                Name = model.Name,
                Priority = model.Priority,
                Status = Enum.Parse<TodoTaskStatus>(model.Status.ToString())
            };
        }
    }
}