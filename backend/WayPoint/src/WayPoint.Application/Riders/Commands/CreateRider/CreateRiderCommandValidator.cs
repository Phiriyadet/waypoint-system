using FluentValidation;
using WayPoint.Domain.Enums;

namespace WayPoint.Application.Riders.Commands.CreateRider;

public class CreateRiderCommandValidator : AbstractValidator<CreateRiderCommand>
{
    public CreateRiderCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Phone)
            .NotEmpty()
            .Matches(@"^\+66\d{9}$")
            .WithMessage("Phone must be in format +66XXXXXXXXX");
        RuleFor(x => x.VehicleType)
            .NotEmpty()
            .Must(v => Enum.TryParse<VehicleType>(v, out _))
            .WithMessage($"VehicleType must be one of: {string.Join(", ", Enum.GetNames<VehicleType>())}");
    }
}
