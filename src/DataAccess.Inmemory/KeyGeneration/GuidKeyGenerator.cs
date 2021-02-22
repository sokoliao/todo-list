using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Inmemory.KeyGeneration
{
    public class GuidKeyGenerator : IKeyGenerator
    {
        public Task<string> NextAsync() => Task.FromResult(Guid.NewGuid().ToString());
    }
}
