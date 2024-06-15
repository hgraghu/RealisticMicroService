using System.ComponentModel.DataAnnotations;

namespace UserService.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
