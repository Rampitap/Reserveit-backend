using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.UpdateOwnerBusiness;

public sealed class UpdateOwnerBusinessCommandValidator : AbstractValidator<UpdateOwnerBusinessCommand>
{
    public UpdateOwnerBusinessCommandValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
        RuleFor(x => x.Data).NotNull();

        RuleFor(x => x.Data!.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Data!.Timezone).NotEmpty().MaximumLength(100);

        RuleFor(x => x.Data)
            .Must(d => d!.OpeningTime == null || d.ClosingTime == null || d.ClosingTime > d.OpeningTime)
            .WithMessage("ClosingTime must be later than OpeningTime.");
    }
}
