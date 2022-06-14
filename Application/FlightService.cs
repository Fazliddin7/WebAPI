using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Infrastructure.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Application
{
    public class FlightService : IFlightService
    {
        private readonly IRepository _repository;
        private readonly IMemoryCache _memoryCache;
        const String cacheKey = "flightList";

        public FlightService(IRepository repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        #region cache
        private MemoryCacheEntryOptions GetCacheExpiryOptions
            => new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };

        private async Task<List<Flight>> GetCache(CancellationToken cancellationToken)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out List<Flight> flightList))
            {
                flightList = (await GetList(cancellationToken)).ToList();
                _memoryCache.Set(cacheKey, flightList, GetCacheExpiryOptions);
            }

            return flightList;
        }

        private void SetCache(List<Flight> flightList)
            => _memoryCache.Set(cacheKey, flightList, GetCacheExpiryOptions);
        #endregion

        private async Task<int> SaveInDb(Flight flight, CancellationToken cancellationToken)
        {
            var result = await _repository.AddAsync(FlightEntity.New(flight), cancellationToken);
            return result.Id;
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

        public async Task<Flight> AddNew(Flight flight, CancellationToken cancellationToken)
        {
            flight.Id = await SaveInDb(flight, cancellationToken);
            var cache = await GetCache(cancellationToken);
            var current = cache.FirstOrDefault(f1 => f1.Id == flight.Id);
            if (current == null)
                cache.Add(flight);
            SetCache(cache);
            return flight; ;
        }

        public async Task<IEnumerable<Flight>> GetAll(String origin, String destination, CancellationToken cancellationToken)
        {
            var flightList = await GetCache(cancellationToken);
            
            return flightList.OrderByDescending(f1 => f1.Arrival)
                .Where(f1 =>
                    (String.Concat(origin) != "" ? f1.Origin == origin : true)
                     &&
                    (String.Concat(destination) != "" ? f1.Destination == destination : true));
        }

        public async Task<Flight> UpdateStatus(int id, FlightStatus status, CancellationToken cancellationToken)
        {
            Flight flight = null;
            var found = await _repository.SingleAsync<FlightEntity>(f1 => f1.Id == id, cancellationToken);

            if (found != null)
            {
                found.Status = status;
                var result = await _repository.UpdateAsync<FlightEntity>(found);
                flight = new Flight()
                {
                    Arrival = result.Arrival,
                    Departure = result.Departure,
                    Destination = result.Destination,
                    Origin = result.Origin,
                    Status = result.Status,
                    Id = result.Id
                };

                var cache = await GetCache(cancellationToken);

                var current = cache.FirstOrDefault(f1 => f1.Id == flight.Id);
                if (current != null)
                    cache.Remove(current);
                cache.Add(flight);

                SetCache(cache);
            }
                
            return flight;
        }

    }
}