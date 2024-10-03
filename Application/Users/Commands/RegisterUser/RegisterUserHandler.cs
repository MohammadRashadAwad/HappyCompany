using Application.Common.Authentication;
using Application.Services;
using Application.Users.Commands.AddUser;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.RegisterUser
{
    public class RegisterUserHandler : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly IUserManagerService _userManagerService;
        private readonly ILogger<RegisterUserHandler> _logger;
        private readonly ITokenGenerator _tokenGenerator;

        public RegisterUserHandler(IUserManagerService userManagerService, ILogger<RegisterUserHandler> logger, ITokenGenerator tokenGenerator)
        {
            _userManagerService = userManagerService;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
                EmailConfirmed = true
            };

            var createUserResult = await _userManagerService.CreateAsync(user, request.Password);
            if (createUserResult.IsFailure)
            {
                _logger.LogError(createUserResult.Error.ToString());
                return Result.Failure<RegisterUserResponse>(createUserResult.Error);
            }

            // assign role to the user
            var assignRoleResult = await _userManagerService.AssignToRoleAsync(user, request.Role);
            if (assignRoleResult.IsFailure)
            {
                _logger.LogError(assignRoleResult.Error.ToString());
                return Result.Failure<RegisterUserResponse>(
                    new("RoleAssignmentFailed", $"Failed to assign role {request.Role}."));
            }

            var accessToken = await _tokenGenerator.GenerateAccessToken(user);
            var refreshToken = await _tokenGenerator.GenerateRefreshToken(user);

            await _userManagerService.SignInAsync(user);

            return new RegisterUserResponse
            {
                UserName = user.FullName,
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                Role = request.Role.ToString()
            };
        }
    }
}
