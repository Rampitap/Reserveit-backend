using MediatR;
using Reserveit.Application.Common.DTOs.AdminManageDtos;

namespace Reserveit.Application.CommandsQueriesForModels.AdminManagement.Queries.GetAdminUserById;

public sealed record GetAdminUserByIdQuery(Guid UserId) : IRequest<AdminUserDto>;
