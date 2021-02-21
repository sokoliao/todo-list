using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace BxTestTask.Stores
{
  public class TodoTaskStore : ITodoTaskStore
  {
    private TodoTaskEntity[] tasks;

    public TodoTaskStore(IOptions<TodoTaskStoreOption> config)
    {
      this.tasks = config.Value.InitialSet ?? new TodoTaskEntity[0];
    }

    public Task<IEnumerable<TodoTaskEntity>> Get() => 
      Task.FromResult(tasks.AsEnumerable());

    public Task Set(Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>> setter)
    {
      this.tasks = setter(this.tasks).ToArray();
      return Task.CompletedTask;
    }

  }
  public interface ITodoTaskStore
  {
    Task<IEnumerable<TodoTaskEntity>> Get();
    Task Set(Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>> setter);
  }
}