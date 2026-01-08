using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetOwnerBusinessStaff;

public sealed class GetOwnerBusinessStaffQueryValidator : AbstractValidator<GetOwnerBusinessStaffQuery>
{
    public GetOwnerBusinessStaffQueryValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
    }
}
