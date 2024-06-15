using AuthenticationService.Common;
using AuthenticationService.Interfaces;
using AuthenticationService.Models;
using AuthenticationService.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> user;
        private readonly IConfiguration config;
        private readonly IMailService mail;
        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMailService mailService)
        {
            user = userManager;
            config = configuration;
            mail = mailService;
        }

        public async Task<UserManagerResponseViewModel> LoginUserAsync(LoginViewModel model)
        {
            var loginuser = await user.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponseViewModel
                {
                    Message = "There is no user with that Email address",
                    IsSuccess = false,
                };
            }

            var result = await user.CheckPasswordAsync(loginuser, model.Password);

            if (!result)
                return new UserManagerResponseViewModel
                {
                    Message = "Invalid password",
                    IsSuccess = false,
                };

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier, loginuser.Id)
            };

            var authSettings = config.GetSection(AuthSettingOptions.AuthOption).Get<AuthSettingOptions>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretCode));

            var token = new JwtSecurityToken(
                issuer: authSettings.IssuerDomain,
                audience: authSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(authSettings.JwtExpiryMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponseViewModel
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo,
            };
        }
    }
}
