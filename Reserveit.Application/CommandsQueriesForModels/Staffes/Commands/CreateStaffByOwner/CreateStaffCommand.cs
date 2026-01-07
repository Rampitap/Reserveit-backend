using MediatR;
using Reserveit.Application.Common.DTOs.StaffDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.CreateStaffByOwner;

public sealed record CreateStaffCommand(CreateStaffAccountDto Data) : IRequest<Guid>;
