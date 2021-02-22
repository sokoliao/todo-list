using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Abstraction
{
    public interface ITodoTaskEntityRepository
    {
        Task<string> CreateAsync(TodoTaskEntity entity);
        Task<IEnumerable<TodoTaskEntity>> GetAll();
        Task UpdateAsync(TodoTaskEntity entity);
        Task DeleteAsync(TodoTaskEntity entity);

    }
}
