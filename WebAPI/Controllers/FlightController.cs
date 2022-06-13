using Application.DTO;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private IMemoryCache _memoryCache;

        public FlightController(IFlightService flightService, IMemoryCache memoryCache)
        {
            _flightService = flightService;
            _memoryCache = memoryCache;
        }

        private MemoryCacheEntryOptions GetCacheExpiryOptions
            => new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };

        private async Task<List<Flight>> GetCache(CancellationToken cancellationToken)
        {
            var cacheKey = "flightList";
            if (!_memoryCache.TryGetValue(cacheKey, out List<Flight> flightList))
            {
                flightList = (await _flightService.GetAll(cancellationToken)).ToList();
                _memoryCache.Set(cacheKey, flightList, GetCacheExpiryOptions);
            }

            return flightList;
        }

        private void SetCache(List<Flight> flightList)
        {
            var cacheKey = "flightList";
            _memoryCache.Set(cacheKey, flightList, GetCacheExpiryOptions);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<FlightDTO>>> Get(FlightGetRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest();

            //TO DO: Impl filter request;

            var flightList = await GetCache(cancellationToken);
            return Ok(flightList.OrderByDescending(f1=> f1.Arrival).Select(f1=> 
                new FlightDTO() 
                { 
                    Departure = f1.Departure,
                    Destination = f1.Destination,
                    Arrival = f1.Arrival,
                    Origin = f1.Origin,
                    Status = f1.Status,
                    Id = f1.Id
                }));
        }

        [HttpPost("create"), Authorize(Roles = "Moderator")]
        public async Task<ActionResult> Create([FromBody] FlightDTO request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest();

            var result = await _flightService.AddNew(
                new Flight()
                { 
                    Arrival = request.Arrival,
                    Departure = request.Departure,
                    Destination = request.Destination,
                    Origin = request.Origin,
                    Status = request.Status
                },
                cancellationToken);

            var cache = await GetCache(cancellationToken);
            var current = cache.FirstOrDefault(f1 => f1.Id == result.Id);
            if (current == null)
                cache.Add(result);
            SetCache(cache);

            return Ok(new FlightDTO
            {
                Departure = result.Departure,
                Destination = result.Destination,
                Arrival = result.Arrival,
                Origin = result.Origin,
                Status = result.Status,
                Id = result.Id
            });
        }

        [HttpPost("changeStatus"), Authorize(Roles = "Moderator")]
        public async Task<ActionResult> ChangeStatus([FromBody] FlightChangeStatusRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest();

            var result = await _flightService.UpdateStatus(request.Id, request.Status,
                cancellationToken);

            if(result == null)
                return NotFound();

            var cache = await GetCache(cancellationToken);

            var current = cache.FirstOrDefault(f1 => f1.Id == result.Id);
            if(current != null)
                cache.Remove(current);
            cache.Add(result);

            SetCache(cache);

            return Ok(result);
        }
    }
}
