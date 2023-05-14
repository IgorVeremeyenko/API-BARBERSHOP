using WebApplication2.Services.Cache;

namespace WebApplication2.Services.Auth {
    public class ValidateOTP {

        private CacheService _cacheService;
        private readonly string cacheEmail = "email_cache_key";
        private readonly string cacheOTP = "otp_cache_key";
        public ValidateOTP(CacheService cacheService) {
            _cacheService = cacheService;
        }
        public bool IsOTPCodeValid(string email, int otpCode) {
            var isEmail = _cacheService.TryGetValueSingle(cacheEmail, email);
            var isValidOTP = _cacheService.TryGetValueSingle(cacheOTP, otpCode);
            
            return isEmail && isValidOTP ? true : false;
        }
    }
}
