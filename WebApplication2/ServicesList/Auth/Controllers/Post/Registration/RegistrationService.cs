using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Auth.Controllers.Post.Registration {
    public class RegistrationService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> Init(
            LoginUser user, 
            GeneratingSaltForPasswordHashing generatingSaltForPasswordHashing,
            CheckUser checkUser,
            HashingPassword hashingPassword
            ) {

            var badRequest = new BadRequestResult();

            var isValid = checkUser.Check(user);

            if (!isValid) return badRequest;

            var okResult = new OkResult();

            // generate salt
            var salt = generatingSaltForPasswordHashing.GenerateSalt();

            // hash password with salt
            var hashedPassword = hashingPassword.HashPassword(user.Password, salt);

            Admin admin = new Admin {
                Name = user.Name,
                Password = hashedPassword,
                Email = user.Email,
                Salt = salt,
            };

            // save user to database
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return okResult;
        }
    }
}
