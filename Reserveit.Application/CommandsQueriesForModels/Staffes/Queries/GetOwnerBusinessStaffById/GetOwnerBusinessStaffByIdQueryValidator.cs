using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetOwnerBusinessStaffById;

public sealed class GetOwnerBusinessStaffByIdQueryValidator : AbstractValidator<GetOwnerBusinessStaffByIdQuery>
{
    public GetOwnerBusinessStaffByIdQueryValidator()
    {
        RuleFor(x => x.BusinessId).NotEmpty();
        RuleFor(x => x.StaffId).NotEmpty();
    }
}
