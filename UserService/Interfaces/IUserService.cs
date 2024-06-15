using UserService.ViewModels;

namespace UserService.Interfaces
{
    public interface IUserService
    {
        Task<UserManagerResponseViewModel> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponseViewModel> ConfirmEmailAsync(string userId, string token);
        Task<UserManagerResponseViewModel> ForgotPasswordAsync(string email);
        Task<UserManagerResponseViewModel> ResetPasswordAsync(ResetPasswordViewModel model);
    }
}
