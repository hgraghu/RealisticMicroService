using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; } = String.Empty;
    }
}
