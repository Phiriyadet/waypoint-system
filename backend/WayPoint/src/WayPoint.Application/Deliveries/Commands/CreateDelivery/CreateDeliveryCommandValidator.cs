using FluentValidation;

namespace WayPoint.Application.Deliveries.Commands.CreateDelivery;

public class CreateDeliveryCommandValidator : AbstractValidator<CreateDeliveryCommand>
{
    public CreateDeliveryCommandValidator()
    {
        RuleFor(x => x.LocationId).NotEmpty();
        RuleFor(x => x.RecipientName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RecipientPhone)
            .NotEmpty()
            .Matches(@"^\+66\d{9}$")
            .WithMessage("Phone must be in format +66XXXXXXXXX");
    }
}
