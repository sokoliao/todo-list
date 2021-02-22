using DataAccess.Abstraction;
using DataAccess.Inmemory.Constraints;
using DataAccess.Inmemory.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Inmemory.Store
{
    public class ConstrantedStoreDecorator : ITodoTaskStore
    {
        private readonly ITodoTaskStore decoratee;
        private readonly IEnumerable<IConstraint> constraints;

        public ConstrantedStoreDecorator(
            ITodoTaskStore decoratee,
            IEnumerable<IConstraint> constraints)
        {
            this.decoratee = decoratee;
            this.constraints = constraints;
        }

        public Task<IEnumerable<TodoTaskEntity>> Get() => decoratee.Get();

        public async Task Set(Func<IEnumerable<TodoTaskEntity>, IEnumerable<TodoTaskEntity>> setter)
        {
            await decoratee.Set(prev =>
            {
                var next = setter(prev).ToArray();
                foreach (var constraint in constraints)
                {
                    constraint.ValidateAndThrow(next);
                }
                return next;
            });
        }
    }
}
