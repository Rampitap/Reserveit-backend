using MediatR;
using Reserveit.Application.Common.DTOs.StaffDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetOwnerBusinessStaffById;

public sealed record GetOwnerBusinessStaffByIdQuery(Guid BusinessId, Guid StaffId)
    : IRequest<OwnerStaffDto>;
