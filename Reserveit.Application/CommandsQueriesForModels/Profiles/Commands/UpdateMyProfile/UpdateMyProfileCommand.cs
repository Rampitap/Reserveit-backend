using MediatR;
using Reserveit.Application.Common.DTOs.UserDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Profiles.Commands.UpdateMyProfile;

public sealed record UpdateMyProfileCommand(UpdateProfileDto Data) : IRequest;
