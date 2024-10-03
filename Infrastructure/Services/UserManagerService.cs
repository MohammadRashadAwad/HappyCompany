using Application.Common.Shared;
using Application.Services;
namespace Infrastructure.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagerService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email) =>
            await _userManager.FindByEmailAsync(email);

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user) =>
            await _userManager.GetRolesAsync(user);
        public async Task SignInAsync(ApplicationUser user) =>
           await _signInManager.SignInAsync(user, isPersistent: false);

        public async Task<Result> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result.Failure(ExtractIdentityErrors(result));
            return Result.Success();
        }

        public async Task<Result> CreateAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return Result.Failure(ExtractIdentityErrors(result));
            return Result.Success();
        }

        public async Task<bool> CheckIfRoleExistsAsync(RoleType role) =>
            await _roleManager.RoleExistsAsync(role.ToString());

        public async Task<Result> AssignToRoleAsync(ApplicationUser user, RoleType role)
        {
            var result = await _userManager.AddToRoleAsync(user, role.ToString());
            if (!result.Succeeded)
                return Result.Failure(ExtractIdentityErrors(result));
            return Result.Success();
        }
        private IdentityErrors ExtractIdentityErrors(IdentityResult result) =>
            new(result.Errors.Select(s => new Error(s.Code, s.Description)).ToArray());
    }

}
