using FluentValidation;

namespace WayPoint.Application.Deliveries.Commands.CompleteDelivery;

public class CompleteDeliveryCommandValidator : AbstractValidator<CompleteDeliveryCommand>
{
    public CompleteDeliveryCommandValidator()
    {
        RuleFor(x => x.DeliveryId).NotEmpty();
        RuleFor(x => x.RiderId).NotEmpty();
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        RuleFor(x => x.AccuracyMeters).GreaterThan(0).LessThanOrEqualTo(1000);
    }
}