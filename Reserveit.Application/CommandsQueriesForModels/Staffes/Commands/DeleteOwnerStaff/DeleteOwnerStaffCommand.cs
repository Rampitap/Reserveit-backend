using MediatR;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.DeleteOwnerStaff;

public sealed record DeleteOwnerStaffCommand(Guid BusinessId, Guid StaffId) : IRequest;
