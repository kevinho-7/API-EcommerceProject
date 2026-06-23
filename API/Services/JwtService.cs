using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenToken(JwtClaimsData data)
    {
        var claims = new[]
        {
            new Claim(
                "id",
                data.id.ToString()
            ),
            new Claim(
                "email",
                data.email!
            ),
            new Claim(
                "role",
                data.role!
            )
        };

        var key = Encoding.UTF8.GetBytes(
            _config["Jwt:Key"]!
        );

        var securityKey = new SymmetricSecurityKey(key);

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        var token = 
            new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );
        
        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}