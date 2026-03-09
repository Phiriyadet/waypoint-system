using FluentValidation;

namespace WayPoint.Application.Locations.Commands.CreateLocation;

public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationCommandValidator()
    {
        RuleFor(x => x.AddressInfo).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Subdistrict).NotEmpty().MaximumLength(100);
        RuleFor(x => x.District).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Province).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .Length(5)
            .Matches(@"^\d{5}$").WithMessage("PostalCode must be 5 digits");
        RuleFor(x => x.MoreInfo).MaximumLength(500).When(x => x.MoreInfo is not null);
        RuleFor(x => x.PlaceName).MaximumLength(200).When(x => x.PlaceName is not null);
        RuleFor(x => x.AccessNotes).MaximumLength(500).When(x => x.AccessNotes is not null);
    }
}
