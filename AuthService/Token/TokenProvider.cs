using System.Security.Claims;
using System.Text;
using AuthService.Entity.Model;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Token;

public class TokenProvider(IConfiguration configuration)
{

    public string Create(User user)
    {
        string secretKey = configuration["Jwt:Secret"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //tạo các claims cho jwt
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            // new Claim("email_verified", "true"),
            new Claim("username", user.Username.ToString())
        };
        
        //thêm danh sách roles vào claims
        if (user.Roles != null)
        {
            claims.AddRange(user.Roles.Select(role => new Claim("roles", role)));
        }
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);
        
        return token;
    }
    
}