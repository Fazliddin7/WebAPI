using MediatR;

namespace Application.Flights.Commands.GetFlight
{
    public class GetFlightCommand: IRequest<List<FlightLookupDTO>>
    {
        public String Origin { get; set; }
        public String Destination { get; set; }
    }
}
