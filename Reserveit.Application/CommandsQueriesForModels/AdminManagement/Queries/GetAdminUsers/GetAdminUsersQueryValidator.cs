using FluentValidation;

namespace Reserveit.Application.CommandsQueriesForModels.AdminManagement.Queries.GetAdminUsers;

public sealed class GetAdminUsersQueryValidator : AbstractValidator<GetAdminUsersQuery>
{
    public GetAdminUsersQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.Q).MaximumLength(200);
    }
}
