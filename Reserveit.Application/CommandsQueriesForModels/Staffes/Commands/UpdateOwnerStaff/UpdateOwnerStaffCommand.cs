using MediatR;
using Reserveit.Application.Common.DTOs.StaffDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.UpdateOwnerStaff;

public sealed record UpdateOwnerStaffCommand(
    Guid BusinessId,
    Guid StaffId,
    UpdateOwnerStaffDto Data) : IRequest;
