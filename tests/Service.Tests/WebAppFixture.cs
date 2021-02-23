using System.Collections.Generic;
using System.Linq;
using DataAccess.Abstraction;
using DataAccess.Inmemory.Stores;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Service;

namespace ServiceTests
{
    public class WebAppFixture : WebApplicationFactory<Program>
    {
        private readonly IEnumerable<TodoTaskEntity> initialSet;

        public WebAppFixture(IEnumerable<TodoTaskEntity> initialSet)
        {
            this.initialSet = initialSet;
        }
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return base.CreateWebHostBuilder();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.Configure<TodoTaskStoreOption>(o => o.InitialSet = initialSet.ToArray());
            });
        }
    }
}