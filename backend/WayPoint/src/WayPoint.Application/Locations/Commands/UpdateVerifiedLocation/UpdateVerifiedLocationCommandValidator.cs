using FluentValidation;

namespace WayPoint.Application.Locations.Commands.UpdateVerifiedLocation;

public class UpdateVerifiedLocationCommandValidator
    : AbstractValidator<UpdateVerifiedLocationCommand>
{
    public UpdateVerifiedLocationCommandValidator()
    {
        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage("LocationId is required");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90 degrees");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180 degrees");

        RuleFor(x => x.OffsetMeters)
            .GreaterThanOrEqualTo(0)
            .WithMessage("OffsetMeters must be 0 or greater");
    }
}
