namespace Infrastructure.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            var rolesCount = await roleManager.Roles.CountAsync();
            if (rolesCount == default)
            {
                await roleManager.CreateAsync(new() { Name = RoleType.Admin.ToString() });
                await roleManager.CreateAsync(new() { Name = RoleType.Management.ToString() });
                await roleManager.CreateAsync(new() { Name = RoleType.Auditor.ToString() });
            }
        }

    }
}