using BussinessLogic.Abstraction.Model;
using System.Threading.Tasks;

namespace BussinessLogic.Abstraction.Handlers
{
    public interface IUpdateCommandHandler
    {
        Task HandleAsync(TodoTask task);
    }
}