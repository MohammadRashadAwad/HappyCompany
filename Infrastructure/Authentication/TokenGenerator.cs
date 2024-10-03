using Application.Common.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
namespace Infrastructure.Authentication
{
    internal class TokenGenerator : ITokenGenerator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSetting _jwt;
        private readonly ILogger<TokenGenerator> _logger;

        public TokenGenerator(UserManager<ApplicationUser> userManager, IOptions<JwtSetting> jwt, ILogger<TokenGenerator> logger)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _logger = logger;
        }

        public async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            try
            {
                var role = await _userManager.GetRolesAsync(user);

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role",role.First())
                };

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwt.Issuer,
                    audience: _jwt.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(_jwt.DurationInMinutes),
                    signingCredentials: signingCredentials);

                return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate access token for user {UserId}.", user.Id);
                throw;
            }
        }


        public async Task<RefreshToken> GenerateRefreshToken(ApplicationUser user)
        {
            try
            {
                var activeRefreshToken = user.RefreshTokens?.FirstOrDefault(t => t.IsActive);

                if (activeRefreshToken is not null)
                    return activeRefreshToken;

                var newRefreshToken = CreateNewRefreshToken(user.Id);

                user.RefreshTokens?.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);
                return newRefreshToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate refresh token for user {UserId}.", user.Id);
                throw;
            }
        }

        private RefreshToken CreateNewRefreshToken(string userId)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                UserId = userId
            };
        }
    }
}
