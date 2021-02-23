using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstraction;
using DataAccess.Inmemory.KeyGeneration;
using DataAccess.Inmemory.Stores;

namespace DataAccess.Inmemory.Repository
{
    public class TodoTaskEntityRepository : ITodoTaskEntityRepository
    {
        private readonly ITodoTaskStore store;
        private readonly IKeyGenerator generator;

        public TodoTaskEntityRepository(
            ITodoTaskStore store,
            IKeyGenerator generator)
        {
            this.store = store;
            this.generator = generator;
        }
        public async Task<string> CreateAsync(TodoTaskEntity entity)
        {
            entity.Id = await generator.NextAsync();
            await store.Set(tasks => tasks.Append(entity));
            return entity.Id;
        }

        public async Task DeleteAsync(TodoTaskEntity entity)
        {
            await store.Set(tasks => tasks.Where(task => task.Id != entity.Id));
        }

        public async Task<IEnumerable<TodoTaskEntity>> GetAll()
        {
            return await store.Get();
        }

        public async Task<TodoTaskEntity> GetById(string id)
        {
            return (await store.Get()).SingleOrDefault(task => task.Id == id);
        }

        public async Task UpdateAsync(TodoTaskEntity entity)
        {
            await store.Set(tasks => tasks
                .Where(task => task.Id != entity.Id)
                .Append(entity));
        }
    }
}
