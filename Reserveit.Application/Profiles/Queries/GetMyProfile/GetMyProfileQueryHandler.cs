using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Reserveit.Application.Common.DTOs.UserDtos;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Entities;

namespace Reserveit.Application.Profiles.Queries.GetMyProfile;

public sealed class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, UserProfileDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ILogger<GetMyProfileQueryHandler> _logger;
    public GetMyProfileQueryHandler(ICurrentUser currentUser, UserManager<User> userManager, IMapper mapper, ILogger<GetMyProfileQueryHandler> logger)
    {
        _currentUser = currentUser;
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserProfileDto> Handle(GetMyProfileQuery request, CancellationToken ct) 
    {
        _logger.LogInformation("Getting currentUser");

        var userId = _currentUser.UserId;

        var user = await _userManager.FindByIdAsync(userId.ToString())
                   ?? throw new UnauthorizedAccessException("User not found");

        var dto = _mapper.Map<UserProfileDto>(user);


        var roles = await _userManager.GetRolesAsync(user);
        dto.Roles = roles.ToList();

        return dto;
    }
}
