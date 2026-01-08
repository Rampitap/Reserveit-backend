using MediatR;
using Reserveit.Application.Common.DTOs.StaffDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetOwnerBusinessStaff;

public sealed record GetOwnerBusinessStaffQuery(Guid BusinessId)
    : IRequest<IReadOnlyList<OwnerStaffDto>>;
