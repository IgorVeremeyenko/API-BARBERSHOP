using System.Text;
using System.Security.Cryptography;

namespace WebApplication2.Services.Cache
{
    public class HashingPassword
    {

        public string HashPassword(string password, string salt)
        {
            var saltedPassword = string.Concat(password, salt);
            using var sha256 = SHA256.Create();
            var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword)));
            return hashedPassword;
        }
    }
}
