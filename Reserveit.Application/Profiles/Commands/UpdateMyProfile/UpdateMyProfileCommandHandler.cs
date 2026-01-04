using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Entities;

namespace Reserveit.Application.Profiles.Commands.UpdateMyProfile;

public sealed class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UpdateMyProfileCommandHandler> _logger;
    private readonly IValidator<UpdateMyProfileCommand> _validator;

    public UpdateMyProfileCommandHandler(ICurrentUser currentUser, UserManager<User> userManager, ILogger<UpdateMyProfileCommandHandler> logger, IValidator<UpdateMyProfileCommand> validator)
    {
        _currentUser = currentUser;
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
    }


    public async Task Handle(UpdateMyProfileCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid)
        {
            _logger.LogWarning("UpdateMyProfile validation failed. UserId={UserId}. Errors={Errors}",
                _currentUser.UserId,
                string.Join("; ", vr.Errors.Select(e => $"{e.PropertyName}:{e.ErrorMessage}")));

            throw new ValidationException(vr.Errors);
        }

        _logger.LogInformation("Updating profile for currentUser");

        var userId = _currentUser.UserId;

        var user = await _userManager.FindByIdAsync(userId.ToString())
                   ?? throw new UnauthorizedAccessException("User not found");

        user.FirstName = request.Data.FirstName.Trim();
        user.LastName = request.Data.LastName.Trim();

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}
