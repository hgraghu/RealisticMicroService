using AuthenticationService.ViewModels;

namespace AuthenticationService.Interfaces
{
    public interface IAuthService
    {
        Task<UserManagerResponseViewModel> LoginUserAsync(LoginViewModel model);
    }
}
