using MediatR;
using Reserveit.Application.Common.DTOs.UserDtos;

namespace Reserveit.Application.Profiles.Queries.GetMyProfile;

public sealed record GetMyProfileQuery : IRequest<UserProfileDto>;
