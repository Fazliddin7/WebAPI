using Domain.Models;
using Domain.Services;
using Infrastructure.Models;
using MediatR;

namespace Application.Flights.Commands.UpdateFlight
{
    public class UpdateFlightCommandHandler: IRequestHandler<UpdateFlightCommand>
    {
        private readonly IRepository _repository;
        private readonly IFlightCache _flightChace;
        public UpdateFlightCommandHandler(IRepository repository, IFlightCache flightChace)
        {
            _repository = repository;
            _flightChace = flightChace;
        }

        public async Task<Unit> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
        {
            var found = await _repository.SingleAsync<FlightEntity>(f1 => f1.Id == request.Id, cancellationToken);
            if (found != null)
            {
                found.Status = request.Status;
                await _repository.UpdateAsync<FlightEntity>(found);

                var flight = new Flight()
                {
                    Arrival = found.Arrival,
                    Departure = found.Departure,
                    Destination = found.Destination,
                    Origin = found.Origin,
                    Status = found.Status,
                    Id = found.Id
                };
                await _flightChace.Replace(flight, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
