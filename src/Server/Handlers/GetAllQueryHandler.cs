using System.Linq;
using System.Threading.Tasks;
using BxTestTask.Stores;

namespace BxTestTask.Handlers
{
  public class GetAllQueryHandler 
    : IQueryHandler<GetAllQuery, GetAllQueryResult>
  {
    private readonly ITodoTaskStore store;

    public GetAllQueryHandler(ITodoTaskStore store)
    {
      this.store = store;
    }
    public async Task<GetAllQueryResult> HandleAsync(GetAllQuery query)
    {
      var tasks = await store.Get();
      return new GetAllQueryResult
      {
        Tasks = tasks
          .Select(entity => entity.ToModel())
          .ToArray()
      };
    }
  }
}