using BussinessLogic.Abstraction.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BussinessLogic.Abstraction.Handlers
{
    public interface IGetAllQueryHandler
    {
        Task<IEnumerable<TodoTask>> HandleAsync();
    }
}