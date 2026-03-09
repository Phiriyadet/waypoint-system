using FluentValidation;

namespace WayPoint.Application.Riders.Commands.UpdateRiderLocation;

public class UpdateRiderLocationCommandValidator
    : AbstractValidator<UpdateRiderLocationCommand>
{
    public UpdateRiderLocationCommandValidator()
    {
        RuleFor(x => x.RiderId).NotEmpty();
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
    }
}
