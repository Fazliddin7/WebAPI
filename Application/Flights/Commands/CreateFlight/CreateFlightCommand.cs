using MediatR;

namespace Application.Flights.Commands.CreateFlight
{
    public class CreateFlightCommand: IRequest<int>
    {
        public String Origin { get; set; }
        public String Destination { get; set; }
        public DateTimeOffset Departure { get; set; }
        public DateTimeOffset Arrival { get; set; }
    }
}
