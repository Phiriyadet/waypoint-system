using FluentValidation;

namespace WayPoint.Application.Deliveries.Commands.StartDelivery;

public class StartDeliveryCommandValidator : AbstractValidator<StartDeliveryCommand>
{
    public StartDeliveryCommandValidator()
    {
        RuleFor(x => x.DeliveryId)
            .NotEmpty()
            .WithMessage("DeliveryId is required");

        RuleFor(x => x.RiderId)
            .NotEmpty()
            .WithMessage("RiderId is required");
    }
}
