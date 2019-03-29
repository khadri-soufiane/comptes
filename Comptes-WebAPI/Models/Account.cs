using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comptes_WebAPI.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public DateTime LastLoginDateTime { get; set; }
    }
}
