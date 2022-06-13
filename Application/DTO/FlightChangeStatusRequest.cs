using Domain.Enums;

namespace Application.DTO
{
    public class FlightChangeStatusRequest
    {
        public int Id { get; set; }
        public FlightStatus Status { get; set; }
    }
}
