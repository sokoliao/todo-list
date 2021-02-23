using BussinessLogic.Abstraction.Model;
using System.Threading.Tasks;

namespace BussinessLogic.Abstraction.Handlers
{
    public interface IDeleteCommandHandler
    {
        Task HandleAsync(string id);
    }
}