using Domain.Common;
using Domain.Enums;
using Domain.Models;

namespace Infrastructure.Models
{
    public class FlightEntity: IBaseEntity
    {
        private FlightEntity() { }

        public FlightEntity(
            String origin,
            String destination,
            DateTimeOffset departure,
            DateTimeOffset arrival,
            FlightStatus status)
        {
            Origin = origin;
            Destination = destination;
            Departure = departure;
            Arrival = arrival;
            Status = status;
        }

        public int Id { get; private set; }

        public String Origin { get; private set; }
        public String Destination { get; private set; }

        public DateTimeOffset Departure { get; private set; }
        public DateTimeOffset Arrival { get; private set; }
        public FlightStatus Status { get; set; }

        public static FlightEntity New(Flight payment)
            => new(payment.Origin, payment.Destination, payment.Departure, payment.Arrival, FlightStatus.InTime);
    }
}
