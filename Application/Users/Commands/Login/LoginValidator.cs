using Application.Services;

namespace Application.Users.Commands.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        private readonly IUserManagerService _userManagerService;
        public LoginValidator(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;

            RuleFor(p => p.Email).NotEmpty()
                .NotNull()
                .EmailAddress()
                .WithMessage("Please enter a valid email address.");

            RuleFor(p => p.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters.")
                .Matches(@"[0-9]")
                .WithMessage("Password must contain at least one digit.")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[\W]")
                .WithMessage("Password must contain at least one special character.");

            RuleFor(p => new { p.Email, p.Password })
                .MustAsync(async (loginInfo, cancellation) => await ValidateLogin(loginInfo.Email, loginInfo.Password))
                .WithMessage("Invalid Email Or Password.");
        }
        private async Task<bool> ValidateLogin(string email, string password)
        {
            var user = await _userManagerService.FindByEmailAsync(email);
            if (user is null)
                return false;

            return await _userManagerService.CheckPasswordAsync(user, password);
        }
    }
}