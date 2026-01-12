using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.CreateOwnerBusiness;

public sealed class CreateOwnerBusinessCommandValidator : AbstractValidator<CreateOwnerBusinessCommand>
{
    public CreateOwnerBusinessCommandValidator()
    {
        RuleFor(x => x.Data).NotNull();
        RuleFor(x => x.Data!.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Data!.Timezone).NotEmpty().MaximumLength(100);

        RuleFor(x => x.Data)
            .Must(d => d!.OpeningTime == null || d.ClosingTime == null || d.ClosingTime > d.OpeningTime)
            .WithMessage("ClosingTime must be later than OpeningTime.");

        RuleFor(x => x.Data!.CategoryId)
            .Must(id => id == null || id.Value != Guid.Empty)
            .WithMessage("CategoryId must be a valid GUID.");
    }
}
