using BussinessLogic.Abstraction.Model;
using DataAccess.Abstraction;
using System;

namespace BussinessLogic.Handlers.Implementation
{
    public static class ModelToEntityMapper
    {
        public static TodoTaskEntity ToEntity(this CreateTodoTaskCommand command)
        {
            return new TodoTaskEntity
            {
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

        public static TodoTaskEntity ToEntity(this TodoTask model)
        {
            return new TodoTaskEntity
            {
                Id = model.Id,
                Name = model.Name,
                Priority = model.Priority,
                Status = Enum.Parse<TodoTaskEntityStatus>(model.Status.ToString())
            };
        }
    }
}