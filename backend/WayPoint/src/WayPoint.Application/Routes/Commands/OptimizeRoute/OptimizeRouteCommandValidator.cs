using FluentValidation;
using WayPoint.Domain.Enums;

namespace WayPoint.Application.Routes.Commands.OptimizeRoute;

public class OptimizeRouteCommandValidator : AbstractValidator<OptimizeRouteCommand>
{
    public OptimizeRouteCommandValidator()
    {
        RuleFor(x => x.RiderId).NotEmpty();
        RuleFor(x => x.DeliveryIds)
            .NotEmpty().WithMessage("DeliveryIds ต้องมีอย่างน้อย 1 รายการ")
            .Must(ids => ids.Count <= 50).WithMessage("DeliveryIds ไม่ควรเกิน 50 รายการ");
        RuleFor(x => x.Strategy)
            .NotEmpty()
            .Must(s => Enum.TryParse<RouteOptimizationStrategy>(s, out _))
            .WithMessage($"Strategy must be one of: {string.Join(", ", Enum.GetNames<RouteOptimizationStrategy>())}");
    }
}
