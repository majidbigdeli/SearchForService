using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Exceptions;
using SearchForApi.Models.Inputs;
using SearchForApi.Services;
using SearchForApi.Utilities;

namespace SearchForApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("auth")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("google")]
        public async Task<ResponseDto<TokenDto>> GoogleLogin(GoogleLoginModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            var token = await _authService.GoogleLogin(model.IdToken, Request.Platform(), model.Device);

            return new ResponseDto<TokenDto>(token);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ResponseDto<Null>> Register(RegisterModel user)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            await _authService.Register(user.Email, user.Password, Request.Platform(), user.Device);

            return new ResponseDto<Null>();
        }

        [AllowAnonymous]
        [HttpPost("register/phone")]
        public async Task<ResponseDto<Null>> RegisterWithPhoneNumber(RegisterWithPhoneModel user)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            await _authService.RegisterWithPhoneNumber(user.PhoneNumber, Request.Platform(), user.Device);

            return new ResponseDto<Null>();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ResponseDto<TokenDto>> Login(LoginModel user)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            var token = await _authService.Login(user.Email, user.Password, user.Device);

            return new ResponseDto<TokenDto>(token);
        }

        [AllowAnonymous]
        [HttpGet("confirm-email/{email}/{token}")]
        public async Task<ActionResult> ConfirmEmail(string email, string token)
        {
            var redirectUrl = await _authService.ConfirmEmail(email, token.UrlDecoded());

            return Redirect(redirectUrl);
        }

        [AllowAnonymous]
        [HttpPost("confirm-phone")]
        public async Task<ResponseDto<TokenDto>> ConfirmPhoneNumber(ConfirmPhoneNumberModel model)
        {
            var token = await _authService.ConfirmPhoneNumber(model.PhoneNumber, model.Token, model.Device);

            return new ResponseDto<TokenDto>(token);
        }

        [HttpPost("change-password")]
        public async Task<ResponseDto<Null>> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            await _authService.ChangePassword(User.Username(), model.NewPassword);

            return new ResponseDto<Null>();
        }

        [AllowAnonymous]
        [HttpPost("forget-password")]
        public async Task<ResponseDto<Null>> ForgetPassword(ForgetPasswordModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            await _authService.ForgetPassword(model.Email);

            return new ResponseDto<Null>();
        }

        [AllowAnonymous]
        [HttpPost("confirm-forget-password")]
        public async Task<ResponseDto<Null>> ConfirmForgetPassword(ConfirmForgetPasswordModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            await _authService.ConfirmForgetPassword(model.Email, model.Token.UrlDecoded(), model.NewPassword);

            return new ResponseDto<Null>();
        }

        [AllowAnonymous]
        [HttpPost("resend-email-confirmation-code")]
        public async Task<ResponseDto<Null>> ResendConfirmationCode(ForgetPasswordModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            await _authService.ResendEmailConfirmationCode(model.Email);

            return new ResponseDto<Null>();
        }

        [AllowAnonymous]
        [HttpPost("resend-phone-confirmation-code")]
        public async Task<ResponseDto<Null>> ResendPhoneNumberConfirmationCode(ResendPhoneNumberConfirmationModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            await _authService.ResendPhoneNumberConfirmationCode(model.PhoneNumber);

            return new ResponseDto<Null>();
        }

        [HttpPost("logout")]
        public async Task<ResponseDto<Null>> Logout(LogoutModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            if (model.DeviceId != null)
                await _authService.LogOut(User.Id(), model.DeviceId);

            return new ResponseDto<Null>();
        }
    }
}

