using Accounts.Models.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Accounts.ViewModels
{
    public class UpdateProfileViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Server Server { get; set; }

        [Required]
        public string InGameCharacter { get; set; }

        [MinLength(7)]
        [MaxLength(20)]
        public string Password { get; set; }

        [MinLength(7)]
        [MaxLength(20)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required]
        public bool Subscribe { get; set; }
    }
}
