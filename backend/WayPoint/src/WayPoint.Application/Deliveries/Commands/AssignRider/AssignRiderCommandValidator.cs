using FluentValidation;

namespace WayPoint.Application.Deliveries.Commands.AssignRider;

public class AssignRiderCommandValidator : AbstractValidator<AssignRiderCommand>
{
    public AssignRiderCommandValidator()
    {
        RuleFor(x => x.DeliveryId).NotEmpty();
        RuleFor(x => x.RiderId).NotEmpty();
    }
}
