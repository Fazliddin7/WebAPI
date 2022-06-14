using FluentValidation;

namespace Application.Flights.Commands.CreateFlight
{
    public class CreateFlightCommandValidator: AbstractValidator<CreateFlightCommand>
    {
        public CreateFlightCommandValidator()
        {
            RuleFor(createFlightCommand =>
                createFlightCommand.Origin).NotEmpty().MaximumLength(256);

            RuleFor(createFlightCommand =>
                createFlightCommand.Destination).NotEmpty().MaximumLength(256);
        }
    }
}
