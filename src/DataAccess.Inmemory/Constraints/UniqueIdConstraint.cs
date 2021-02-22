using DataAccess.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Inmemory.Constraints
{
    public class UniqueIdConstraint : IConstraint
    {
        public void ValidateAndThrow(IEnumerable<TodoTaskEntity> entities)
        {
            var duplicate = entities
                .GroupBy(
                    entity => entity.Id,
                    (id, duplicates) => new
                    {
                        id,
                        count = duplicates.Count()
                    })
                .Where(entity => entity.count > 1)
                .FirstOrDefault();

            if (duplicate != null)
            {
                throw new ValidationException(
                    $"Violation of unique {nameof(TodoTaskEntity.Id)} - {duplicate.id}");
            }
        }
    }
}
