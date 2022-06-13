using Domain.Enums;
using Domain.Models;

namespace Domain.Services
{
    public interface IFlightServiceFilter
    {
        string Origin { get; set; }
        string Destination { get; set; }
    }

    public interface IFlightService
    {
        Task<Flight> AddNew(Flight flight, CancellationToken cancellationToken);

        Task<IEnumerable<Flight>> GetAll(CancellationToken cancellationToken);

        Task<Flight> UpdateStatus(int id, FlightStatus status, CancellationToken cancellationToken);
    }
}
