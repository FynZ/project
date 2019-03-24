using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAuthApp.ViewModels
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
        public Server Server { get; set; }

        [Required]
        [MinLength(7)]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        [MinLength(7)]
        [MaxLength(20)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
