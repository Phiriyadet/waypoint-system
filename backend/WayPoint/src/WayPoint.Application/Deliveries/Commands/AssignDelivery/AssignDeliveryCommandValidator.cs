using FluentValidation;

namespace WayPoint.Application.Deliveries.Commands.AssignRider;

public class AssignDeliveryCommandValidator : AbstractValidator<AssignDeliveryCommand>
{
    public AssignDeliveryCommandValidator()
    {
        RuleFor(x => x.DeliveryId).NotEmpty();
        RuleFor(x => x.RiderId).NotEmpty();
    }
}
