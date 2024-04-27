using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace QWiz.Helpers.Authentication.TokenService;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));
        var signinCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            configuration["JWT:ValidIssuer"],
            configuration["JWT:ValidAudience"],
            claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: signinCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rang = RandomNumberGenerator.Create();
        rang.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase
            ))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }
}