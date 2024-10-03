using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool Active { get; set; }
        public bool IsDefaultUser { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}
