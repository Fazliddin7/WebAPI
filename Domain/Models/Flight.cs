using Domain.Enums;

namespace Domain.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public String Origin { get; set; }
        public String Destination { get; set; }
        public DateTimeOffset Departure { get; set; }
        public DateTimeOffset Arrival { get; set; }
        public FlightStatus Status { get; set; }
    }
}