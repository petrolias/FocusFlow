using FocusFlow.WebApi.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FocusFlow.WebApi.Tests
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
                // Configure additional test services here
            });
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ProjectsController>();
            services.AddTransient<TasksController>();
            services.AddTransient<DashboardController>();
            services.AddTransient<AuthController>();
        }
    }

   
}
