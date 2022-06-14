using Application.Common.Exceptions;
using Application.Flights.Commands.UpdateFlight;
using Infrastructure.Models;
using System.Threading;
using System.Threading.Tasks;
using Tests.Common;
using Xunit;

namespace Tests.Flights.Commands
{
    public class UpdateFlightCommandHandlerTest: TestCommandBase
    {
        [Fact]
        public async Task UpdateFlightCommandHandler_Success()
        {
            // Arrange
            var handler = new UpdateFlightCommandHandler(this.efRepository, this.flightCache);
            var status = Domain.Enums.FlightStatus.Delayed;

            // Act
            await handler.Handle(
                new UpdateFlightCommand
                {
                    Id = FlightContextFactory.FlightIdForUpdate,
                    Status = status
                },
                CancellationToken.None);
            
            // Assert
            Assert.NotNull(await this.efRepository.SingleAsync<FlightEntity>(
                f1 => f1.Id == FlightContextFactory.FlightIdForUpdate && 
                f1.Status == status));
        }

        [Fact]
        public async Task UpdateFlightCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new UpdateFlightCommandHandler(this.efRepository, this.flightCache);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateFlightCommand
                    {
                        Id = 0,
                        Status = Domain.Enums.FlightStatus.Delayed
                    },
                    CancellationToken.None));
        }
    }
}
