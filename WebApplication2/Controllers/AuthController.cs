using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplication2.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApplication2.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly MyDatabaseContext _dbContextUsers;

        private readonly IConfiguration _configuration;

        public class LoginUser 
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
        }

        private class UserData 
        {
           public string? Name { get; set; }
           public string? Id { get; set; }
        }

        public AuthController(MyDatabaseContext dbContextUsers, IConfiguration configuration) {
            _dbContextUsers = dbContextUsers;
            _configuration = configuration;
        }

        private string HashPassword(string password, string salt) {
            var saltedPassword = string.Concat(password, salt);
            using var sha256 = SHA256.Create();
            var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword)));
            return hashedPassword;
        }

        private string GenerateSalt() {
            var random = new Random();
            var salt = new byte[16];
            random.NextBytes(salt);
            return Convert.ToBase64String(salt);
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] LoginUser model) {

            var dbUser = await _dbContextUsers.Admins.FirstOrDefaultAsync(u => u.Name == model.UserName);

            if (dbUser != null) {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString())
                };
                
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Invalid credentials");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginUser user) {
            if (user == null || string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Invalid request");

            // generate salt
            var salt = GenerateSalt();

            // hash password with salt
            var hashedPassword = HashPassword(user.Password, salt);

            Admin admin = new Admin 
            {
                Name = user.UserName,
                Password = hashedPassword,
                Salt = salt,
            };

            // save user to database
            _dbContextUsers.Admins.Add(admin);
            await _dbContextUsers.SaveChangesAsync();

            return Ok(JsonSerializer.Serialize("User registered successfully"));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser request) {
            if (request == null || string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Invalid request");

            // get user from database
            var dbUser = await _dbContextUsers.Admins.FirstOrDefaultAsync(u => u.Name == request.UserName);

            if (dbUser == null)
                return NotFound("User not found");

            // hash password with user's salt and compare with hashed password in database
            var hashedPassword = HashPassword(request.Password, dbUser.Salt);

            if (hashedPassword != dbUser.Password)
                return BadRequest("Invalid credentials");

            // generate JWT token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim("ID",dbUser.Id.ToString()),
                new Claim("NAME", dbUser.Name)
            };

            var key = Encoding.UTF8.GetBytes(_configuration.GetValue("Jwt:Key", "")) ;
            var issuer = _configuration.GetValue<string>("Jwt:Issuer", "");
            var audience = _configuration.GetValue<string>("Jwt:Audience", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new {
                tokenString
            });
        }

        [AllowAnonymous]
        [HttpGet("validate")]
        public async Task<IActionResult> ValidateTokenAsync(string token) {
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
                var newUser = new UserData {
                    Id = userId,
                    Name = userName
                };
                var user = await _dbContextUsers.Admins.FindAsync(int.Parse(userId));

                if (userId == null) {
                    return NotFound();
                }

                return Ok(newUser);
            }
            catch {
                return BadRequest("Token is expired");
            }
        }
    }
}
