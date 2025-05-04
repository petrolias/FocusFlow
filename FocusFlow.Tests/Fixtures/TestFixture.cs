using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FocusFlow.Core;
namespace FocusFlow.Tests.Fixtures
{
    public class TestFixture
    {
        public ServiceProvider ServiceProvider { get; }

        public TestFixture()
        {
            var services = new ServiceCollection();
            var dbName = Guid.NewGuid().ToString();
            services.AddDependencies(options => options.UseInMemoryDatabase(dbName));                        
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}