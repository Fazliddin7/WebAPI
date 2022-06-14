using Domain.Models;

namespace Domain.Services
{
    public interface IFlightCache
    {
        Task<List<Flight>> GetCache(CancellationToken cancellationToken);
        void SetCache(List<Flight> flightList);

        Task Replace(Flight flight, CancellationToken cancellationToken);

        Task Add(Flight flight, CancellationToken cancellationToken);
    }
}
