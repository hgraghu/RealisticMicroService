using UserService.Interfaces;
using UserService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService user;
        private readonly IMailService mail;
        private readonly IConfiguration config;
        public UserController(IUserService userService, IMailService mailService, IConfiguration configuration)
        {
            user = userService;
            mail = mailService;
            config = configuration;
        }

        // api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await user.RegisterUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result); // Status Code: 200 

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid"); // Status code: 400
        }

        // api/auth/confirmemail?userid&token
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await user.ConfirmEmailAsync(userId, token);

            if (result.IsSuccess)
            {
                return Redirect($"{config["AppUrl"]}/ConfirmEmail.html");
            }

            return BadRequest(result);
        }

        // api/auth/forgetpassword
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotpassworViewModel forgotpassword)
        {
            if (forgotpassword == null)
                return NotFound();

            var result = await user.ForgotPasswordAsync(forgotpassword.EmailId);

            if (result.IsSuccess)
                return Ok(result); // 200

            return BadRequest(result); // 400
        }

        // api/auth/resetpassword
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await user.ResetPasswordAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }

    }
}

