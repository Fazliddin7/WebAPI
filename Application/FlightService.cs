using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Infrastructure.Models;

namespace Application
{
    public class FlightService : IFlightService
    {
        private readonly IRepository _repository;

        public FlightService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Flight> AddNew(Flight flight, CancellationToken cancellationToken)
        {
            flight.Id = await SaveInDb(flight, cancellationToken);
            return flight; ;
        }

        private async Task<int> SaveInDb(Flight flight, CancellationToken cancellationToken)
        {
            var result = await _repository.AddAsync(FlightEntity.New(flight), cancellationToken);
            return result.Id;
        }

        public async Task<IEnumerable<Flight>> GetAll(CancellationToken cancellationToken)
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
            }
                
            return flight;
        }

    }
}