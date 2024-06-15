using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UserService.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = String.Empty;
        [Required, MaxLength(50)]
        public string LastName { get; set; } = String.Empty;
        //public string CompanyId { get; set; } = String.Empty;
        //public Company ?Company { get; set; }
    }
}
