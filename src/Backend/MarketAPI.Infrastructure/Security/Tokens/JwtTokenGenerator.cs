using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace MarketAPI.Infrastructure.Security.Tokens;

internal class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly string _signingKey;
    private readonly int _expirationMinutes;

    public JwtTokenGenerator(string signingKey, int expirationMinutes)
    {
        _signingKey = signingKey;
        _expirationMinutes = expirationMinutes;
    }

    public string Generate(User user)
    {
        try
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_signingKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
                signingCredentials: credentials,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}