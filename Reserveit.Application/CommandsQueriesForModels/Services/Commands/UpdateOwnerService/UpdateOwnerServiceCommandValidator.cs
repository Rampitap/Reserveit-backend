using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.UpdateOwnerService;

public sealed class UpdateOwnerServiceCommandValidator : AbstractValidator<UpdateOwnerServiceCommand>
{
    public UpdateOwnerServiceCommandValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
        RuleFor(x => x.ServiceId).NotEmpty();
        RuleFor(x => x.Data).NotNull();

        RuleFor(x => x.Data.Name).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Data.Description).MaximumLength(500);
        RuleFor(x => x.Data.DurationMinutes).InclusiveBetween(5, 600);
        RuleFor(x => x.Data.Price).GreaterThanOrEqualTo(0).When(x => x.Data.Price.HasValue);
    }
}
