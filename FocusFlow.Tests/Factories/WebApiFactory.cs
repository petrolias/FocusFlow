using FocusFlow.Core;
using FocusFlow.WebApi.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Tests.Factories
{
    public class WebApiFactory<TStartup> :
        WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {            
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<Context>));
                if (descriptor != null)
                    services.Remove(descriptor);
                var dbName = Guid.NewGuid().ToString();
                services.AddDbContext<Context>(options => options.UseInMemoryDatabase(dbName));
                services.AddTransient<ProjectsController>();
                services.AddTransient<AuthController>();
            });
        }
    }
}