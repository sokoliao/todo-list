using System.Threading.Tasks;

namespace BxTestTask.Handlers
{
  public interface IQueryHandler<TQuery, TResult>
  {
    Task<TResult> HandleAsync(TQuery query);
  }
}