using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApplication2.Models;
using System.Text.Json;
using WebApplication2.Services.Cache;
using WebApplication2.Services;
using WebApplication2.Services.Auth;
using Microsoft.Extensions.Caching.Memory;
using WebApplication2.Models.Auth;

namespace WebApplication2.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly MyDatabaseContext _dbContextUsers;

        private readonly IConfiguration _configuration;

        private readonly HashingPassword _hash = new HashingPassword();

        private readonly GeneratingSaltForPasswordHashing _generationSalt = new GeneratingSaltForPasswordHashing();

        private readonly CacheService _cacheService;

        private readonly string cacheEmail = "email_cache_key";
        private readonly string cacheIdAdmin = "admin_id_cache_key";

        public AuthController(MyDatabaseContext dbContextUsers, IConfiguration configuration, CacheService cacheService) {
            _dbContextUsers = dbContextUsers;
            _configuration = configuration;
            _cacheService = cacheService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginUser user) {
            if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Invalid request");

            // generate salt
            var salt = _generationSalt.GenerateSalt();

            // hash password with salt
            var hashedPassword = _hash.HashPassword(user.Password, salt);

            Admin admin = new Admin 
            {
                Name = user.Name,
                Password = hashedPassword,
                Email = user.Email,
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
            if (request == null || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Invalid request");

            // get user from database
            var dbUser = await _dbContextUsers.Admins.FirstOrDefaultAsync(u => u.Name == request.Name);

            if (dbUser == null)
                return NotFound("User not found");

            // hash password with user's salt and compare with hashed password in database
            var hashedPassword = _hash.HashPassword(request.Password, dbUser.Salt);

            if (hashedPassword != dbUser.Password)
                return BadRequest("Invalid credentials");

            // generate JWT token
            GenerateToken tokens = new GenerateToken();

            var tokenString = tokens.ClaimsInit(dbUser, _configuration, request.Name);

            return Ok(new {
                tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("sendCode")]
        public async Task<IActionResult> ResetPassword(ResetPassword model) {
            if (model == null || string.IsNullOrWhiteSpace(model.Login) || string.IsNullOrWhiteSpace(model.Email))
                return BadRequest("Invalid request");
            try {

                var user = await _dbContextUsers.Admins.FirstOrDefaultAsync(u => (u.Name == model.Login) && (u.Email == model.Email));
                if (user == null) {
                    return BadRequest(JsonSerializer.Serialize("Пользователя не найдено в базе данных"));
                    /*return Unauthorized(new { code = "401", message = "Unauthorized request" });*/

                }

                GenerateAndSendOTPOnEmail otp = new GenerateAndSendOTPOnEmail(model.Email, _cacheService);
                var isSentEmail = otp.SendOTPEmail();
                if (isSentEmail) {
                    _cacheService.Set(cacheIdAdmin, user.Id, TimeSpan.FromMinutes(10));
                    return Ok(JsonSerializer.Serialize($"Вам на почту, {model.Email}, отправлен код"));
                }
                else {
                    return BadRequest(JsonSerializer.Serialize("Не удалось отправить код"));
                }
            }
            
            catch (Exception e) {

                await Console.Out.WriteLineAsync(e.Message);
                throw;
            }

            
        }
       
        [AllowAnonymous]
        [HttpPost]
        [Route("validateOtp/{otp}")]
        public IActionResult ValidateOtp(int otp) {
            ValidateOTP validateOTP = new ValidateOTP(_cacheService);
            var email = _cacheService.Get<string>(cacheEmail);
            var isValid = validateOTP.IsOTPCodeValid(email, otp);
            if (isValid) {
                var id = _cacheService.Get<int>(cacheIdAdmin);
                return Ok(new { code = $"{id}", message = "ID" });
            }
            else {
                return Unauthorized(JsonSerializer.Serialize("Неверный код подтверждения"));
            }
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
                var newUser = new UserDataForValidateToken {
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
