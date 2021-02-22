using DataAccess.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Inmemory.Constraints
{
    public interface IConstraint
    {
        void ValidateAndThrow(IEnumerable<TodoTaskEntity> entities);
    }
}
