using Application.Services;
using Application.Users.Commands.AddUser;
using FluentValidation;

namespace Application.Users.Commands.RegisterUser
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly IUserManagerService _userManagerService;

        public RegisterUserValidator(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;


            RuleFor(p => p.Email).NotEmpty()
                .NotNull()
                .EmailAddress()
                .WithMessage("Please enter a valid email address.")
                .MustAsync(async (email, cancellation) => await IsUniqueEmail(email))
                .WithMessage("This email is already registered. Please use a different email address.");

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

            RuleFor(p => p.FullName).NotEmpty()
                .MinimumLength(2)
                .WithMessage("Full Name must be at least 2 characters.")
                .Must(IsValidName)
                .WithMessage("Please Inter valid name ");

            RuleFor(p => p.Role)
                .NotEmpty()
                .MustAsync(async (role, cancellation) => await IsValidRole(role))
                .WithMessage("The specified role is invalid or does not exist.");

        }

        private bool IsValidName(string fullName) => !fullName.Any(c => char.IsDigit(c));
        private async Task<bool> IsUniqueEmail(string email) =>
            await _userManagerService.FindByEmailAsync(email) is null ? true : false;

        private async Task<bool> IsValidRole(RoleType role) =>
            await _userManagerService.CheckIfRoleExistsAsync(role);

    }

}
