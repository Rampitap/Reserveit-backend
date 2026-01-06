using MediatR;
using Reserveit.Application.Common.DTOs.StaffDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetPublicBusinessStaff;

public sealed record GetPublicBusinessStaffQuery(Guid BusinessId) : IRequest<IReadOnlyList<PublicStaffDto>>;
