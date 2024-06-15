using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Common;
using UserService.Interfaces;
using UserService.Models;
using UserService.ViewModels;

namespace UserService.Services
{
    public class UsersService : IUserService
    {
        private readonly UserManager<ApplicationUser> user;
        private readonly IConfiguration config;
        private readonly IMailService mail;
        public UsersService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMailService mailService)
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
        public async Task<UserManagerResponseViewModel> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
                throw new NullReferenceException("Register Model is null");

            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponseViewModel
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };

            var identityUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            };

            var result = await user.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                var confirmEmailToken = await user.GenerateEmailConfirmationTokenAsync(identityUser);

                var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                string url = $"{config["AppUrl"]}/api/auth/confirmemail?userid={identityUser.Id}&token={validEmailToken}";

                await mail.SendEmailAsync(identityUser.Email, "Confirm your email", $"<h1>Welcome to BlackFowl</h1>" +
                    $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");


                return new UserManagerResponseViewModel
                {
                    Message = "User created successfully!",
                    IsSuccess = true,
                };
            }

            return new UserManagerResponseViewModel
            {
                Message = "User did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };

        }
        public async Task<UserManagerResponseViewModel> ConfirmEmailAsync(string userId, string token)
        {
            var confirmuser = await user.FindByIdAsync(userId);
            if (user == null)
                return new UserManagerResponseViewModel
                {
                    IsSuccess = false,
                    Message = "User not found"
                };

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await user.ConfirmEmailAsync(confirmuser, normalToken);

            if (result.Succeeded)
            {
                return new UserManagerResponseViewModel
                {
                    Message = "Email confirmed successfully!",
                    IsSuccess = true,
                };
            }
            return new UserManagerResponseViewModel
            {
                IsSuccess = false,
                Message = "Email did not confirm",
                Errors = result.Errors.Select(e => e.Description)
            };
        }
        public async Task<UserManagerResponseViewModel> ForgotPasswordAsync(string email)
        {
            var forgetuser = await user.FindByEmailAsync(email);
            if (user == null)
                return new UserManagerResponseViewModel
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };

            var token = await user.GeneratePasswordResetTokenAsync(forgetuser);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{config["AppUrl"]}/ResetPassword?email={email}&token={validToken}";

            await mail.SendEmailAsync(email, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
                $"<p>To reset your password <a href='{url}'>Click here</a></p>");

            return new UserManagerResponseViewModel
            {
                IsSuccess = true,
                Message = "Reset password URL has been sent to the email successfully!"
            };
        }
        public async Task<UserManagerResponseViewModel> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var resetuser = await user.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserManagerResponseViewModel
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };

            if (model.NewPassword != model.ConfirmPassword)
                return new UserManagerResponseViewModel
                {
                    IsSuccess = false,
                    Message = "Password doesn't match its confirmation",
                };

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await user.ResetPasswordAsync(resetuser, normalToken, model.NewPassword);

            if (result.Succeeded)
                return new UserManagerResponseViewModel
                {
                    Message = "Password has been reset successfully!",
                    IsSuccess = true,
                };

            return new UserManagerResponseViewModel
            {
                Message = "Something went wrong",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

    }
}
