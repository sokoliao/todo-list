using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Inmemory.KeyGeneration
{
    public interface IKeyGenerator
    {
        Task<string> NextAsync();
    }
}
