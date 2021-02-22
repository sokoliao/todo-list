using BussinessLogic.Abstraction.Handlers;
using BussinessLogic.Abstraction.Model;
using DataAccess.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BussinessLogic.Handlers.Implementation
{
    public class GetAllQueryHandler : IGetAllQueryHandler
    {
        private readonly ITodoTaskEntityRepository repository;

        public GetAllQueryHandler(ITodoTaskEntityRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<TodoTask>> HandleAsync() =>
            (await repository.GetAll()).Select(entity => entity.ToModel());
    }
}