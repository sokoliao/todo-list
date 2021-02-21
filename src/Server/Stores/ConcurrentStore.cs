using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BxTestTask.Stores
{
  public class ConcurrentStore : ITodoTaskStore
  {
    private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    private readonly ITodoTaskStore decoratee;

    public ConcurrentStore(ITodoTaskStore decoratee)
    {
      this.decoratee = decoratee;
    }
    public Task<IEnumerable<TodoTaskEntity>> Get() => decoratee.Get();

    public async Task Set(Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>> setter)
    {
      await semaphore.WaitAsync();
      try
      {
        await decoratee.Set(setter);
      }
      finally
      {
        semaphore.Release();
      }
    }
  }
}