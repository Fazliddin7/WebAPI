using Domain.Models;
using Domain.Services;
using Infrastructure.Models;
using MediatR;

namespace Application.Flights.Commands.CreateFlight
{
    public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, int>
    {
        private readonly IRepository _repository;
        private readonly IFlightCache _flightChace;
        public CreateFlightCommandHandler(IRepository repository, IFlightCache flightChace)
        {
            _repository = repository;
            _flightChace = flightChace;
        }

        public async Task<int> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
        {
            var result = await SaveInDb(
                new Flight () 
                { 
                    Arrival = request.Arrival,
                    Departure = request.Departure,
                    Destination = request.Destination,
                    Origin = request.Destination
                }
                , cancellationToken);
            return result;
        }

        private async Task<int> SaveInDb(Flight flight, CancellationToken cancellationToken)
        {
            var result = await _repository.AddAsync(FlightEntity.New(flight), cancellationToken);
            flight.Id = result.Id;
            await _flightChace.Add(flight, cancellationToken);
            return result.Id;
        }
    }
}
