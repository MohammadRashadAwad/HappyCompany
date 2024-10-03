namespace Infrastructure.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var hasDefaultUser = await userManager.Users.AnyAsync(p => p.IsDefaultUser);
            if (!hasDefaultUser)
            {
                ApplicationUser defaultuser = new()
                {
                    UserName = "admin",
                    Email = "admin@happywarehouse.com",
                    FullName = "Mohammad Awad",
                    PhoneNumber = "962788304304",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Active = true,
                    IsDefaultUser = true,
                };
                await userManager.CreateAsync(defaultuser, "P@ssw0rd");
                await userManager.AddToRoleAsync(defaultuser, RoleType.Admin.ToString());
            }
        }
    }
}
