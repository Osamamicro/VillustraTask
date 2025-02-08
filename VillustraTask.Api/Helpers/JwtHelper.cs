using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VillustraTask.Api.Models;

namespace VillustraTask.Api.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(Userlogin user, IConfiguration configuration)
        {
            try
            {
                var jwtSettings = configuration.GetSection("JwtSettings");

                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];
                var secretKey = jwtSettings["Secret"];
                if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
                {
                    throw new ArgumentException("Invalid or missing JWT Secret Key. Ensure it is at least 32 characters long.");
                }

                if (!int.TryParse(jwtSettings["ExpiryInMinutes"], out int expiryInMinutes) || expiryInMinutes <= 0)
                {
                    throw new ArgumentException("JWT ExpiryInMinutes must be a valid positive integer.");
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("fullName", user.FullName),
                    new Claim("designationId", user.DesignationId?.ToString() ?? "0") // Ensure default value
                };

                var token = new JwtSecurityToken(
                    issuer,
                    audience,
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error generating JWT token: {ex.Message}");
            }
        }
    }
}
