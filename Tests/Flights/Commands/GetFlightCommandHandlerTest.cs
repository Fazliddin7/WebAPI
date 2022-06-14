using Application.Flights.Commands.GetFlight;
using System.Threading;
using System.Threading.Tasks;
using Tests.Common;
using Xunit;

namespace Tests.Flights.Commands
{
    public class GetFlightCommandHandlerTest: TestCommandBase
    {
        [Fact]
        public async Task GetFlughtCommandHandler_Found()
        {
            // Arrange
            var handler = new GetFlightCommandHandler(this.efRepository, this.flightCache);

            // Act
            var result = await handler.Handle(
                new GetFlightCommand
                {
                    Destination = FlightContextFactory.FlightDestination,
                    Origin = FlightContextFactory.FlightOrigin
                },
                CancellationToken.None);

            // Assert
            Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task GetFlughtCommandHandler_NotFound()
        {
            // Arrange
            var handler = new GetFlightCommandHandler(this.efRepository, this.flightCache);
            var destination = "destination not found value";
            var origin = "origin not found value";

            // Act
            var result = await handler.Handle(
                new GetFlightCommand
                {
                    Destination = destination,
                    Origin = origin
                },
                CancellationToken.None);

            // Assert
            Assert.False(result.Count > 0);
        }
    }
}
