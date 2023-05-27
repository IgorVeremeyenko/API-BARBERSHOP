using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication2.Models;

namespace WebApplication2.Services {
    public class GenerateToken {

        private List<Claim>? _claims;

        public string ClaimsInit(Admin dbUser, IConfiguration configuration, string UserName) 
        {
            _claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
                new Claim(ClaimTypes.Name, UserName),
                new Claim("ID",dbUser.Id.ToString()),
                new Claim("NAME", dbUser.Name)
            };

            var key = Encoding.UTF8.GetBytes(configuration.GetValue("Jwt:Key", ""));
            var issuer = configuration.GetValue<string>("Jwt:Issuer", "");
            var audience = configuration.GetValue<string>("Jwt:Audience", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(_claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        

        
    }
}
