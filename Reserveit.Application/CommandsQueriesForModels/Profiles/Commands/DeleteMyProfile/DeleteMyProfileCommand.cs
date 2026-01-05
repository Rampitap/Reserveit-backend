using MediatR;
using Reserveit.Application.Common.DTOs.UserDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Profiles.Commands.DeleteMyProfile;

public sealed record DeleteMyProfileCommand(DeleteMyAccountRequestDto Data) : IRequest;
