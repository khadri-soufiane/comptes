using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Comptes_WebAPI.Models
{
    public class ResetPasswordViewModel
    {
        [DisplayName("ID"), Required]
        public int Id { get; set; }

        [DisplayName("Username"), Required]
        public string Username { get; set; }

        [DisplayName("Current Password"), Required]
        public string CurrentPassword { get; set; }

        [DisplayName("New Password"), Required]
        public string NewPassword { get; set; }

        [DisplayName("Confirm Password"), Required]
        public string ConfirmPassword { get; set; }
    }
}
