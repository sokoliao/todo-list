using DataAccess.Abstraction;
using DataAccess.Inmemory.Constraints;
using DataAccess.Inmemory.KeyGeneration;
using DataAccess.Inmemory.Repository;
using DataAccess.Inmemory.Store;
using DataAccess.Inmemory.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Inmemory.Extenstions
{
    public static class DependencyInjection
    {
        public const string SECTION = "Store";
        public static IServiceCollection AddInmemoryStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TodoTaskStoreOption>(o => configuration.GetSection(SECTION).Bind(o));
            services.AddSingleton<IConstraint, UniqueIdConstraint>();
            services.AddSingleton<IConstraint, UniqueNameConstraint>();
            services.AddSingleton<ITodoTaskStore>(services =>
              new ConcurrentStoreDecorator(
                new ConstrantedStoreDecorator(
                    new TodoTaskStore(
                        services.GetService<IOptions<TodoTaskStoreOption>>()),
                    services.GetService<IEnumerable<IConstraint>>())));
            services.AddSingleton<IKeyGenerator, GuidKeyGenerator>();
            services.AddSingleton<ITodoTaskEntityRepository, TodoTaskEntityRepository>();

            return services;
        }
    }
}
