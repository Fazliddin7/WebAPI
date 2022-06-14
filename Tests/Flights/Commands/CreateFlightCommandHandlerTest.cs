using Application.Flights.Commands.CreateFlight;
using Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tests.Common;
using Xunit;

namespace Tests.Flights.Commands
{
    public class CreateFlightCommandHandlerTest: TestCommandBase
    {
        [Fact]
        public async Task CreateFlughtCommandHandler_Success()
        {
            // Arrange
            var handler = new CreateFlightCommandHandler(this.efRepository, this.flightCache);
            var departure = new DateTimeOffset(2022, 06, 14, 16, 13, 15, 5, new TimeSpan());
            var arrival = new DateTimeOffset(2022, 06, 14, 20, 13, 15, 5, new TimeSpan());
            var destination = "destination value";
            var origin = "origin value";

            // Act
            var flightId = await handler.Handle(
                new CreateFlightCommand
                {
                    Departure = departure,
                    Arrival = arrival,
                    Destination = destination,
                    Origin = origin
                },
                CancellationToken.None);
            
            // Assert
            Assert.NotNull(await this.efRepository.SingleAsync<FlightEntity>(
                f1 => f1.Id == flightId && f1.Departure == departure && f1.Arrival == arrival));
        }
    }
}
