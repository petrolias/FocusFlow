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
            builder.UseEnvironment("Testing");
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<Context>));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddDbContext<Context>(options => options.UseInMemoryDatabase("TestDb"));
                services.AddTransient<ProjectsController>();
                services.AddTransient<AuthController>();
            });
        }
    }
}