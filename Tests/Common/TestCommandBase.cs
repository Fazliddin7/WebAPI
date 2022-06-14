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
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        public TestCommandBase()
        {
            _context = FlightContextFactory.Create();
            efRepository = new EfRepository(_context);

            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            _memoryCache = serviceProvider.GetService<IMemoryCache>();
            flightCache = new FlightCache(efRepository, _memoryCache);
            
        }

        public void Dispose()
        {
            _memoryCache.Dispose();
            FlightContextFactory.Destroy(_context);
        }
    }
}
