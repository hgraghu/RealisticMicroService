using AuthenticationService.Interfaces;
using AuthenticationService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService user;
        private readonly IMailService mail;
        public AuthController(IAuthService userService, IMailService mailService)
        {
            user = userService;
            mail = mailService;
        }

        // api/auth/login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await user.LoginUserAsync(model);

                if (result.IsSuccess)
                {
                    await mail.SendEmailAsync(model.Email, "New login", "<h1>Hey!, new login to your account noticed</h1><p>New login to your account at " + DateTime.Now + "</p>");
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }
    }
}

