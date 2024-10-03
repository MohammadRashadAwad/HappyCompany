namespace Application.Users.Commands.AddUser
{
    public class RegisterUserCommand : ICommand<RegisterUserResponse>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public RoleType Role { get; set; }

    }
}
