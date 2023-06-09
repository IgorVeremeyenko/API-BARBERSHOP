using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApplication2.Models;
using WebApplication2.Models.Auth;
using WebApplication2.Services.Auth;
using WebApplication2.Services.Cache;

namespace WebApplication2.ServicesList.Auth.Controllers.Post.SendCode {
    public class SendCodeService {

        private readonly MyDatabaseContext _context = new MyDatabaseContext();

        public async Task<IActionResult> InitCode(
            ResetPassword model, 
            CacheService _cacheService
            ) {

            var user = await _context.Admins.FirstOrDefaultAsync(u => (u.Name == model.Login) && (u.Email == model.Email));
            if (user == null) {
                return new BadRequestObjectResult(JsonSerializer.Serialize("Пользователя не найдено в базе данных"));               

            }

            GenerateAndSendOTPOnEmail otp = new GenerateAndSendOTPOnEmail(model.Email, _cacheService);
            var isSentEmail = otp.SendOTPEmail();
            if (isSentEmail) {
                var okRes = new OkObjectResult(JsonSerializer.Serialize($"Вам на почту, {model.Email}, отправлен код"));
                string idAdmin = user.Id.ToString();
                _cacheService.Set(idAdmin, user.Id, TimeSpan.FromMinutes(10));
                return okRes;
            }
            else {
                return new BadRequestObjectResult(JsonSerializer.Serialize("Не удалось отправить код"));
            }
        }
    }
}
