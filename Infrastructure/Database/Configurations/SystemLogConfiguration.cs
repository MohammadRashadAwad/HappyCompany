using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    internal class SystemLogConfiguration : IEntityTypeConfiguration<SystemLog>
    {
        public void Configure(EntityTypeBuilder<SystemLog> builder)
        {
            builder.ToTable("SystemLogs");
            builder.Property(e => e.Id).HasColumnName("Id");
            builder.Property(e => e.Timestamp).HasColumnName("Timestamp");
            builder.Property(e => e.Level).HasColumnName("Level");
            builder.Property(e => e.RenderedMessage).HasColumnName("RenderedMessage");
            builder.Property(e => e.Exception).HasColumnName("Exception");
            builder.Property(e => e.Properties).HasColumnName("Properties");
        }
    }
}
