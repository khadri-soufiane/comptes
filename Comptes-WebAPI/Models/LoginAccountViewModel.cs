using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Comptes_WebAPI.Models
{
    public class LoginAccountViewModel
    {
        [DisplayName("Username"), Required]
        public string Username { get; set; }

        [DisplayName("Password"), Required]
        public string Password { get; set; }
    }
}
