using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services.Cache;
using WebApplication2.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace WebApplication2.ServicesList.Auth.Controllers.Post.Login {
    public class LoginService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> InitLogin(
            LoginUser user,  
            IConfiguration _configuration,
            CheckUser checkUser,
            HashingPassword hashingPassword
            ) {

            var notFound = new NotFoundResult();

            var badRequest = new BadRequestResult();

            var isValid = checkUser.Check(user);

            if(!isValid) { return badRequest; }

            // get user from database
            var dbUser = await _context.Admins.FirstOrDefaultAsync(u => u.Name == user.Name);

            if (dbUser == null)
                return notFound;

            // hash password with user's salt and compare with hashed password in database
            var hashedPassword = hashingPassword.HashPassword(user.Password, dbUser.Salt);

            if (hashedPassword != dbUser.Password)
                return badRequest;

            GenerateToken tokens = new GenerateToken();

            var tokenString = tokens.ClaimsInit(dbUser, _configuration, user.Name);

            var okResult = new OkObjectResult(JsonSerializer.Serialize(tokenString));

            return okResult;

        }
    }
}
