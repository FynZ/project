using Accounts.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
