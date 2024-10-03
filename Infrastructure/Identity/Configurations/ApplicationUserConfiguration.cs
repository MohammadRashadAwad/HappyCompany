using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Identity.Configurations
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(p => p.FullName)
                 .HasMaxLength(30)
                 .IsRequired();

            builder.OwnsMany(a => a.RefreshTokens, builder =>
            {
                builder.WithOwner(rt => rt.User)
                          .HasForeignKey(rt => rt.UserId);

                builder.Property(rt => rt.Token)
                         .IsRequired();

                builder.Property(rt => rt.ExpiresOn)
                         .IsRequired();

                builder.Property(rt => rt.CreatedOn)
                         .IsRequired();

                builder.Property(rt => rt.RevokeOn)
                         .IsRequired(false);

                builder.HasKey(rt => rt.Token);
            });

        }
    }
}
