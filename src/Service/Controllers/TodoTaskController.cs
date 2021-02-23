using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogic.Abstraction.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Abstraction.Model;
using Service.Filters;
using Service.Mappers;

namespace Service.Controllers
{
    [ApiController]
    [OverrideModelValidation]
    [Route("tasks")]
    public class TodoTaskController : ControllerBase
    {
        private readonly ILogger<TodoTaskController> logger;
        private readonly ICreateTodoTaskCommandHandler createTodoTaskHandler;
        private readonly IGetAllQueryHandler getAllHandler;
        private readonly IDeleteCommandHandler deleteHandler;
        private readonly IUpdateCommandHandler updateHandler;

        public TodoTaskController(
            ILogger<TodoTaskController> logger,
            ICreateTodoTaskCommandHandler createTodoTaskHandler,
            IGetAllQueryHandler getAllHandler,
            IDeleteCommandHandler deleteHandler,
            IUpdateCommandHandler updateHandler)
        {
            this.logger = logger;
            this.createTodoTaskHandler = createTodoTaskHandler;
            this.getAllHandler = getAllHandler;
            this.deleteHandler = deleteHandler;
            this.updateHandler = updateHandler;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<TodoTaskDto>> GetAllAsync()
        {
            return (await getAllHandler.HandleAsync())
                   .Select(task => task.ToDto());
        }

        [HttpPost("new")]
        public async Task<string> CreateAsync([FromBody] CreateTodoTaskDto task)
        {
            var command = task.ToCommand();
            return await createTodoTaskHandler.HandleAsync(command);
        }

        [HttpDelete("delete/{id}")]
        public async Task Delete(string id)
        {
            await deleteHandler.HandleAsync(id);
        }

        [HttpPut("update")]
        public async Task UpdateAsync([FromBody] TodoTaskDto task)
        {
            await updateHandler.HandleAsync(task.ToModel());
        }
    }
}
