using MediatR;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.AddStaffToService;

public sealed record AddStaffToServiceCommand(Guid BusinessId, Guid ServiceId, Guid StaffId) : IRequest;
