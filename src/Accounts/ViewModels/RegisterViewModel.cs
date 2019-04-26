using System.ComponentModel.DataAnnotations;
using Accounts.Models.Enumerations;

namespace Accounts.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Character { get; set; }

        [Required]
        public Server Server { get; set; }

        [Required]
        [MinLength(7)]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        [MinLength(7)]
        [MaxLength(20)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required]
        public bool Subscribe { get; set; }
    }
}
