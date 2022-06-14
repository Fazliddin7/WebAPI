using Domain.Enums;
using MediatR;

namespace Application.Flights.Commands.UpdateFlight
{
    public class UpdateFlightCommand: IRequest
    {
        public int Id { get; set; }
        public FlightStatus Status { get; set; }
    }
}
