using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Comptes_WebAPI.Models
{
    public class EditAccountViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("UserName")]
        public string UserName { get; set; }

        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; }

        [DisplayName("New Password")]
        public string NewPassword { get; set; }

        [DisplayName("New Password Again")]
        public string NewPasswordAgain { get; set; }

        [DisplayName("Registration Date")]
        public DateTime RegistrationDate { get; set; }

        [DisplayName("Last Login Date")]
        public DateTime LastLogin { get; set; }

        public static EditAccountViewModel FromUser(Account account)
        {
            var model = new EditAccountViewModel();
            model.Id = account.Id;
            model.Name = account.Name;
            model.RegistrationDate = account.RegistrationDateTime;
            model.LastLogin = account.LastLoginDateTime;

            return model;
        }
    }
}
