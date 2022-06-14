using Application.Flights.Commands.CreateFlight;
using Application.Flights.Commands.GetFlight;
using Application.Flights.Commands.UpdateFlight;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ApiControllerBase
    {
        public FlightController(IMediator mediator) : base(mediator) { }

        [HttpPost("getAll"), Authorize]
        public async Task<ActionResult<IEnumerable<FlightLookupDTO>>> GetAll([FromBody] GetFlightCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }


        [HttpPost("create"), Authorize(Roles = "Moderator")]
        public async Task<ActionResult<int>> Create([FromBody] CreateFlightCommand request, CancellationToken cancellationToken)
        {
            var flightId =  await _mediator.Send(request, cancellationToken);
            return Ok(flightId);
        }

        [HttpPost("changeStatus"), Authorize(Roles = "Moderator")]
        public async Task<ActionResult> ChangeStatus([FromBody] UpdateFlightCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }
    }
}
