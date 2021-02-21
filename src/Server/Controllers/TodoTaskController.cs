using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BxTestTask.Handlers;
using BxTestTask.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BxTestTask.Controllers
{
  [ApiController]
  [OverrideModelValidation]
  [Route("tasks")]
  public class TodoTaskController : ControllerBase
  {
    private readonly ILogger<TodoTaskController> logger;
    private readonly ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult> createTodoTaskHandler;
    private readonly IQueryHandler<GetAllQuery, GetAllQueryResult> getAllHandler;
    private readonly ICommandHandler<DeleteCommand> deleteHandler;
    private readonly ICommandHandler<UpdateCommand> updateHandler;

    public TodoTaskController(
      ILogger<TodoTaskController> logger,
      ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult> createTodoTaskHandler,
      IQueryHandler<GetAllQuery, GetAllQueryResult> getAllHandler,
      ICommandHandler<DeleteCommand> deleteHandler,
      ICommandHandler<UpdateCommand> updateHandler)
    {
      this.logger = logger;
      this.createTodoTaskHandler = createTodoTaskHandler;
      this.getAllHandler = getAllHandler;
      this.deleteHandler = deleteHandler;
      this.updateHandler = updateHandler;
    }

    [HttpGet("all")]
    public async Task<IEnumerable<TodoTaskModel>> GetAllAsync()
    {
      var query = new GetAllQuery();
      var result = await getAllHandler.HandleAsync(query);
      return result.Tasks.Select(task => task.ToModel());
    }

    [HttpPost("new")]
    public async Task<CreateTodoTaskResultModel> CreateAsync(
      [FromBody] CreateTodoTaskModel task)
    {
      var command = task.ToCommand();
      var result = await createTodoTaskHandler.HandleAsync(command);
      return new CreateTodoTaskResultModel
      {
        Id = result.Id
      };
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task Delete(Guid id)
    {
      await deleteHandler.HandleAsync(new DeleteCommand { Id = id });
    }

    [HttpPut("update/{id:guid}")]
    public async Task UpdateAsync(Guid id, [FromBody] UpdateTodoTaskModel model)
    {
      var command = model.ToCommand(id);
      await updateHandler.HandleAsync(command);
    }
  }
}
