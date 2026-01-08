using MediatR;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.RemoveStaffFromService;

public sealed record RemoveStaffFromServiceCommand(Guid BusinessId, Guid ServiceId, Guid StaffId) : IRequest;
