using FluentValidation;
using WayPoint.Domain.Enums;

namespace WayPoint.Application.Riders.Commands.UpdateRiderStatus;

public class UpdateRiderStatusCommandValidator
    : AbstractValidator<UpdateRiderStatusCommand>
{
    public UpdateRiderStatusCommandValidator()
    {
        RuleFor(x => x.RiderId).NotEmpty();
        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(s => Enum.TryParse<RiderStatus>(s, out _))
            .WithMessage($"Status must be one of: {string.Join(", ", Enum.GetNames<RiderStatus>())}");
    }
}
