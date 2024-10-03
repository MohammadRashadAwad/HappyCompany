using Application.Common.Authentication;
using Application.Services;
using Microsoft.Extensions.Logging;
namespace Application.Users.Commands.Login
{
    public class LoginHandler : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserManagerService _userService;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(IUserManagerService userService, ITokenGenerator tokenGenerator, ILogger<LoginHandler> logger)
        {
            _userService = userService;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.FindByEmailAsync(request.Email);
            var roles = await _userService.GetRolesAsync(user);
            var accessToken = await _tokenGenerator.GenerateAccessToken(user);
            var refreshToken = await _tokenGenerator.GenerateRefreshToken(user);
            await _userService.SignInAsync(user);
            _logger.LogInformation("User {Username} logged in successfully", user.FullName);
            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                Role = roles.First(),
                UserId = user.Id,
                UserName = user.FullName
            };
        }
    }
}
