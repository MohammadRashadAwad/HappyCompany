namespace Application.Users.Commands.Login
{
    public class LoginCommand : ICommand<LoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
