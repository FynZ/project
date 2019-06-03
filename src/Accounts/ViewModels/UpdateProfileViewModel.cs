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
        public string InGameName { get; set; }

        [Required]
        public bool Subscribed { get; set; }
    }
}
