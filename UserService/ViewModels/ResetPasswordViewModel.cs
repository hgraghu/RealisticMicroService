using System.ComponentModel.DataAnnotations;

namespace UserService.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class ForgotpassworViewModel
    {
        [Required]
        [EmailAddress]
        public string EmailId { get; set; } = string.Empty;
    }
}
