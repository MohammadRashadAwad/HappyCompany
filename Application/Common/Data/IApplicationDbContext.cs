using Microsoft.EntityFrameworkCore;

namespace Application.Common.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
    }
}
