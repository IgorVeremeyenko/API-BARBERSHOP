using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services.Cache;
using WebApplication2.Models.Auth;
using WebApplication2.Services;
using WebApplication2.ServicesList.Auth.Controllers.Post;
using WebApplication2.ServicesList.Auth.Controllers.Post.Registration;
using WebApplication2.ServicesList.Auth.Controllers.Get.Validate;
using WebApplication2.ServicesList.Auth.Controllers.Post.SendCode;
using WebApplication2.ServicesList.Auth.Controllers.Post.Login;
using WebApplication2.ServicesList.Auth.Controllers.Post.ValidateOTP;

namespace WebApplication2.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly HashingPassword _hash;

        private readonly GeneratingSaltForPasswordHashing _generationSalt;

        private readonly IConfiguration _configuration;

        private readonly CacheService _cacheService;

        private readonly CheckUser _checkUser;

        private readonly RegistrationService _registrationService;

        private readonly ValidateTokenService _validateToken;

        private readonly SendCodeService _sendCodeService;

        private readonly LoginService _loginService;

        private readonly ValidateOtpService _validateOtpService;

        public AuthController(
            IConfiguration configuration, 
            CacheService cacheService,
            GeneratingSaltForPasswordHashing generatingSaltForPasswordHashing,
            HashingPassword hashingPassword,
            CheckUser checkUser,
            RegistrationService registrationService,
            ValidateTokenService validateTokenService,
            SendCodeService sendCodeService,
            LoginService loginService,
            ValidateOtpService validateOtpService
            ) {
            _configuration = configuration;
            _cacheService = cacheService;
            _hash = hashingPassword;
            _generationSalt = generatingSaltForPasswordHashing;
            _checkUser = checkUser;
            _registrationService = registrationService;
            _validateToken = validateTokenService;
            _sendCodeService = sendCodeService;
            _loginService = loginService;
            _validateOtpService = validateOtpService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginUser user) {

            return await _registrationService.Init(user, _generationSalt,_checkUser,_hash);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser request) {

            return await _loginService.InitLogin(request, _configuration, _checkUser, _hash);
           
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("sendCode")]
        public async Task<IActionResult> ResetPassword(ResetPassword model) {

            return await _sendCodeService.InitCode(model, _cacheService);
        }
       
        [AllowAnonymous]
        [HttpPost]
        [Route("validateOtp/{otp}")]
        public IActionResult ValidateOtp(int otp, string email) {

            return _validateOtpService.InitOtp(_cacheService, email, otp);
        }

        [AllowAnonymous]
        [HttpGet("validate")]
        public async Task<IActionResult> ValidateTokenAsync(string token) {

            return await _validateToken.InitValidate(token, _configuration);
        }
    }
}
