using BussinessLogic.Abstraction.Model;
using System.Threading.Tasks;

namespace BussinessLogic.Abstraction.Handlers
{
    public interface ICreateTodoTaskCommandHandler
    {
        Task<string> HandleAsync(CreateTodoTaskCommand command);
    }
}