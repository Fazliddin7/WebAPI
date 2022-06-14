using Domain.Models;
using Domain.Services;
using Infrastructure.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Application
{
    public class FlightCache: IFlightCache
    {
        private readonly IRepository _repository;
        private readonly IMemoryCache _memoryCache;
        const String cacheKey = "flightList";
        public FlightCache(IRepository repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        private MemoryCacheEntryOptions GetCacheExpiryOptions
            => new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };

        public async Task<List<Flight>> GetCache(CancellationToken cancellationToken)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out List<Flight> flightList))
            {
                flightList = (await GetList(cancellationToken)).ToList();
                _memoryCache.Set(cacheKey, flightList, GetCacheExpiryOptions);
            }

            return flightList;
        }

        public void SetCache(List<Flight> flightList)
            => _memoryCache.Set(cacheKey, flightList, GetCacheExpiryOptions);

        public async Task Replace(Flight flight, CancellationToken cancellationToken)
        {
            var cache = await GetCache(cancellationToken);
            var current = cache.FirstOrDefault(f1 => f1.Id == flight.Id);

            if (current != null)
                cache.Remove(current);

            cache.Add(flight);

            SetCache(cache);
        }

        public async Task Add(Flight flight, CancellationToken cancellationToken)
        {
            var cache = await GetCache(cancellationToken);
            var current = cache.FirstOrDefault(f1 => f1.Id == flight.Id);
            if (current == null)
                cache.Add(flight);
            SetCache(cache);
        }

        private async Task<IEnumerable<Flight>> GetList(CancellationToken cancellationToken)
        {
            var foundItems = await _repository.ListAsync<FlightEntity>(cancellationToken);
            var result = foundItems.Select(i =>
            new Flight()
            {
                Id = i.Id,
                Arrival = i.Arrival,
                Destination = i.Destination,
                Departure = i.Departure,
                Origin = i.Origin,
                Status = i.Status
            });
            return result;

        }

    }
}
