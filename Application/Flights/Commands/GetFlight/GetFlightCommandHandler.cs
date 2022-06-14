using Domain.Services;
using MediatR;

namespace Application.Flights.Commands.GetFlight
{
    public class GetFlightCommandHandler: IRequestHandler<GetFlightCommand, List<FlightLookupDTO>>
    {
        private readonly IRepository _repository;
        private readonly IFlightCache _flightChace;
        public GetFlightCommandHandler(IRepository repository, IFlightCache flightChace)
        {
            _repository = repository;
            _flightChace = flightChace;
        }

        public async Task<List<FlightLookupDTO>> Handle(GetFlightCommand request, CancellationToken cancellationToken)
        {
            var flightList = await _flightChace.GetCache(cancellationToken);

            return flightList.OrderByDescending(f1 => f1.Arrival)
                .Where(f1 =>
                    (String.Concat(request.Origin) != "" ? f1.Origin == request.Origin : true)
                     &&
                    (String.Concat(request.Destination) != "" ? f1.Destination == request.Destination : true))
                .Select(f2 => new FlightLookupDTO()
                {
                    Arrival = f2.Arrival,
                    Departure = f2.Departure,
                    Destination = f2.Destination,
                    Origin = f2.Origin,
                    Status = f2.Status,
                    Id = f2.Id
                }).ToList();
        }
    }
}
