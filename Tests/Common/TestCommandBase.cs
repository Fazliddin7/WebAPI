using Application;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Common
{
    public class TestCommandBase
    {
        protected readonly FlightCache flightCache;
        protected readonly EfRepository efRepository;
        public TestCommandBase()
        {
            efRepository = new EfRepository(FlightContextFactory.Create());

            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            flightCache = new FlightCache(efRepository, memoryCache);
        }
    }
}
