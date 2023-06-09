using WebApplication2.Services.Cache;

namespace WebApplication2.Services.Auth {
    public class ValidateOTP {

        private CacheService _cacheService;
        public ValidateOTP(CacheService cacheService) {
            _cacheService = cacheService;
        }
        public bool IsOTPCodeValid(string email, int otpCode) {
            int otpFromCache = _cacheService.Get<int>(email);     
            return otpCode == otpFromCache;
        }
    }
}
