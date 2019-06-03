using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Models.Enumerations;

namespace Accounts.ViewModels
{
    public class UpdateProfileWithPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Server Server { get; set; }

        [Required]
        public string InGameName { get; set; }

        [MinLength(7)]
        [MaxLength(20)]
        public string Password { get; set; }

        [MinLength(7)]
        [MaxLength(20)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required]
        public bool Subscribed { get; set; }
    }
}
