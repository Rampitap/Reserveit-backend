using MediatR;
using Reserveit.Application.Common.DTOs.UserDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Profiles.Queries.GetMyProfile;

public sealed record GetMyProfileQuery : IRequest<UserProfileDto>;
