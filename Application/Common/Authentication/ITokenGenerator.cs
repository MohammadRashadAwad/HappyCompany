
namespace Application.Common.Authentication
{
    public interface ITokenGenerator
    {
        Task<string> GenerateAccessToken(ApplicationUser user);
        Task<RefreshToken> GenerateRefreshToken(ApplicationUser user);
    }
}
