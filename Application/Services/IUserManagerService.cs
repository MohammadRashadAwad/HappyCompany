namespace Application.Services
{
    public interface IUserManagerService
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<Result> UpdateUserAsync(ApplicationUser user);
        Task SignInAsync(ApplicationUser user);
        Task<Result> CreateAsync(ApplicationUser user, string password);
        Task<bool> CheckIfRoleExistsAsync(RoleType role);
        Task<Result> AssignToRoleAsync(ApplicationUser user, RoleType role);
    }
}
