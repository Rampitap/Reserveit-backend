using MediatR;
using Reserveit.Application.Common.DTOs.UserDtos;

namespace Reserveit.Application.Profiles.Commands.UpdateMyPassword;

public sealed record UpdateMyPasswordCommand(ChangePasswordRequestDto Data) : IRequest;
