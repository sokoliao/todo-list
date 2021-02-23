using DataAccess.Abstraction;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Inmemory.Constraints
{
    public class UniqueNameConstraint : IConstraint
    {
        public void ValidateAndThrow(IEnumerable<TodoTaskEntity> entities)
        {
            var duplicate = entities
                .GroupBy(
                    entity => entity.Name,
                    (name, duplicates) => new
                    {
                        name,
                        count = duplicates.Count()
                    })
                .Where(entity => entity.count > 1)
                .FirstOrDefault();

            if (duplicate != null)
            {
                throw new TodoListValidationException(
                    $"Violation of unique {nameof(TodoTaskEntity.Name)} - {duplicate.name}");
            }
        }
    }
}
