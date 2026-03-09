using FluentValidation;

namespace WayPoint.Application.Deliveries.Commands.CancelDelivery;

public class CancelDeliveryCommandValidator : AbstractValidator<CancelDeliveryCommand>
{
    public CancelDeliveryCommandValidator()
    {
        RuleFor(x => x.DeliveryId).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}
