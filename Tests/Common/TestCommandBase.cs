using Application;
using Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Tests.Common
{
    public class TestCommandBase: IDisposable
    {
        protected readonly FlightCache flightCache;
        protected readonly EfRepository efRepository;
        private readonly ApplicationDbContext context;
        public TestCommandBase()
        {
            context = FlightContextFactory.Create();
            efRepository = new EfRepository(context);

            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            flightCache = new FlightCache(efRepository, memoryCache);
        }

        public void Dispose()
        {
            FlightContextFactory.Destroy(context);
        }
    }
}
