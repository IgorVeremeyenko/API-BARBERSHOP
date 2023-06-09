using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplication2.Models;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Auth.Controllers.Post.ValidateOTP {
    public class ValidateOtpService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public IActionResult InitOtp(CacheService _cacheService, string email, int otp) {

            Services.Auth.ValidateOTP validateOTP = new Services.Auth.ValidateOTP(_cacheService);            
            var isValid = validateOTP.IsOTPCodeValid(email, otp);
            if (isValid) {
                int id = _context.Admins.Select(x => x.Id).FirstOrDefault();
                if(id != -1) {
                    return new OkObjectResult(new { code = $"{id}", message = "ID" });
                }
                else {
                    return new UnauthorizedResult();
                }
            }
            else {
                return new UnauthorizedObjectResult(JsonSerializer.Serialize("Неверный код подтверждения"));
            }
        }
    }
}
