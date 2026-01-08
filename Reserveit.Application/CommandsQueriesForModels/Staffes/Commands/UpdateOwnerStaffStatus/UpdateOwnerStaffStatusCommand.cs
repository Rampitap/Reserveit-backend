using MediatR;
using Reserveit.Application.Common.DTOs.StaffDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.UpdateOwnerStaffStatus;

public sealed record UpdateOwnerStaffStatusCommand(Guid BusinessId, Guid StaffId, UpdateStaffStatusDto Data) : IRequest;
