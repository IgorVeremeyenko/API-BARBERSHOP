using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApplication2.Models;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Auth.Controllers.Get.Validate {
    public class ValidateTokenService {

        private readonly CacheService _cacheService;

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> InitValidate(string token, IConfiguration _configuration) {
            try {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.ReadJwtToken(token);
                var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key", ""));
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration.GetValue<string>("Jwt:Issuer", ""),
                    ValidAudience = _configuration.GetValue<string>("Jwt:Audience", ""),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var userId = jwt.Claims.First(c => c.Type == "ID").Value;
                var userName = jwt.Claims.First(n => n.Type == "NAME").Value;
                var newUser = new UserDataForValidateToken {
                    Id = userId,
                    Name = userName
                };
                var user = await _context.Admins.FindAsync(int.Parse(userId));

                if (userId == null) {
                    return new NotFoundResult();
                }

                return new OkObjectResult(newUser);
            }
            catch {
                return new BadRequestObjectResult("Token is expired");
            }
        }
    }
}
