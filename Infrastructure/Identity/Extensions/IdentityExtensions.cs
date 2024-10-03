namespace Infrastructure.Identity.Extensions
{
    public static class IdentityExtensions
    {
        public static ModelBuilder RenameDefaultIdentityTablesExtension(this ModelBuilder builder)
        {
            const string identitySchema = "security";
            builder.Entity<ApplicationUser>().ToTable("Users", identitySchema);
            builder.Entity<IdentityRole>().ToTable("Roles", identitySchema);
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", identitySchema);
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim", identitySchema);
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim", identitySchema);
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", identitySchema);
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken", identitySchema);
            return builder;
        }
    }
}
