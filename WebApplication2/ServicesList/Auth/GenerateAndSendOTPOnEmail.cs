using System.Net.Mail;
using System.Net;
using WebApplication2.Services.Cache;

namespace WebApplication2.Services.Auth {
    public class GenerateAndSendOTPOnEmail {
        private string _email;
        private readonly string cacheEmail = "email_cache_key";
        private readonly string cacheOTP = "otp_cache_key";
        private CacheService _cacheService;
        public GenerateAndSendOTPOnEmail(string email, CacheService cacheService) {
            _email = email;
            _cacheService = cacheService;
        }

        private int GenerateOTP() {
            Random random = new Random();
            int otp = random.Next(100000, 999999);
            return otp;
        }

        public bool SendOTPEmail() {
            int otpCode = GenerateOTP();
            try {
                SmtpClient smtpClient = new SmtpClient("smtp.mail.ru", 587);
                smtpClient.UseDefaultCredentials = false;
                //
                smtpClient.Credentials = new NetworkCredential("igor_veremeyenko@mail.ru", "ayqwRJpUK2F9ji0MPAFg");
                smtpClient.EnableSsl = true;
                    
                MailMessage mailMessage = new MailMessage();
                string htmlBody = $@"
                                    <html>
                                    <body>
                                        <h2>Восстановление пароля</h2>
                                        <p>Вы запросили сброс пароля. Ваш код подтверждения:</p>
                                        <h1 style='font-size: 48px;'>{otpCode}</h1>
                                        <p>Введите этот код на странице сброса пароля. Он будет действителен в течение 10 минут</p>
                                    </body>
                                    </html>";

                mailMessage.Body = htmlBody;
                mailMessage.IsBodyHtml = true;
                mailMessage.From = new MailAddress("igor_veremeyenko@mail.ru");
                mailMessage.To.Add(_email);
                mailMessage.Subject = "Код OTP для сброса пароля в приложении \"Cut Master\"";
                _cacheService.Set(_email, otpCode, TimeSpan.FromMinutes(10));
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception) {
                return false;
            }
        }
    }
}
