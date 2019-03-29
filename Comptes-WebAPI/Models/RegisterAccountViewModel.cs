using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Comptes_WebAPI.Models
{
    public class RegisterAccountViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Name"), Required]
        public string Name { get; set; }

        [DisplayName("Username"), Required]
        public string Username { get; set; }

        [DisplayName("Email"), Required]
        public string Email { get; set; }

        [DisplayName("Password"), Required]
        public string Password { get; set; }

        [DisplayName("Confirm Password"), Required]
        public string RepeatPassword { get; set; }

        
    }
}
