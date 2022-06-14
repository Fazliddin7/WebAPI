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

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<IEnumerable<FlightDTO>>> Get(FlightGetRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest();

            var flightList = await _flightService.GetAll(request.Origin, request.Destination, cancellationToken);

            return Ok(
                flightList.Select(f1=> 
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

            return Ok(result);
        }
    }
}
