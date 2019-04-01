using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Comptes_WebAPI.Models;
using Comptes_WebAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Comptes_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComptesAPIController : ControllerBase
    {
        IAccountRepository accountRepository;
        public ComptesAPIController(IAccountRepository _accountRepository)
        {
            accountRepository = _accountRepository;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpGet]
        [Route("GetAccounts")]
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                var accounts = await accountRepository.GetAll();
                if (accounts == null)
                {
                    return NotFound();
                }

                return Ok(accounts);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetById/{accountId}")]
        public async Task<IActionResult> GetById(int? accountId)
        {
            if (accountId == null)
            {
                return BadRequest();
            }

            try
            {
                var account = await accountRepository.GetById(accountId);

                if (account == null)
                {
                    return NotFound();
                }

                return Ok(account);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetByUsername/{username}")]
        public IActionResult GetByUsername(string username)
        {
            if (username == null)
            {
                return BadRequest();
            }

            try
            {
                var account = accountRepository.GetByUserName(username);

                if (account == null)
                {
                    return NotFound();
                }

                return Ok(account);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddAccount")]
        public async Task<IActionResult> AddAccount([FromBody]RegisterAccountViewModel model)
        {
            model.Name = model.Name.Trim();
            model.Password = model.Password.Trim();
            model.RepeatPassword = model.RepeatPassword.Trim();

            var targetAccount = accountRepository.GetByUserName(model.Username);
            //db.Accounts.SingleOrDefault(a => a.Username == model.Username);

            if (targetAccount != null)
            {
                return BadRequest("Compte Existe déja.");
            }


            if (!model.Password.Equals(model.RepeatPassword))
            {
                return BadRequest();
            }

            var hasher = new PasswordHasher<Account>();
            var account = new Account { Name = model.Name, Username = model.Username, Email = model.Email, RegistrationDateTime = DateTime.Now };
            account.PasswordHash = hasher.HashPassword(account, model.Password);


            await accountRepository.AddAccount(account);


            return Ok(account);
        }

        [HttpPut]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            var hasher = new PasswordHasher<Account>();

            var targetAccount = accountRepository.GetByUserName(model.Username);
            if (targetAccount == null)
            {
                return NotFound("Le compte n'existe pas");
            }

            model.CurrentPassword = model.CurrentPassword.Trim();
            model.NewPassword = model.NewPassword.Trim();
            model.ConfirmPassword = model.ConfirmPassword.Trim();

            if (!model.NewPassword.Equals(model.ConfirmPassword, StringComparison.CurrentCultureIgnoreCase))
            {
                return BadRequest("Le mot de passe et le nouveau mot de passe ne correspondent pas.");
            }

            var result = hasher.VerifyHashedPassword(targetAccount, targetAccount.PasswordHash, model.CurrentPassword);

            if (result != PasswordVerificationResult.Success)
            {
                return BadRequest("Mot de passe incorrect.");
            }

            targetAccount.PasswordHash = hasher.HashPassword(targetAccount, model.NewPassword);

            await accountRepository.ResetPassword(targetAccount);

            return Ok(targetAccount);
        }


        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromBody]EditAccountViewModel model)
        {
            var hasher = new PasswordHasher<Account>();

            if (!ModelState.IsValid)
            {
                return BadRequest("Informations de compte non valides.");
            }

            var targetAccount = accountRepository.GetByUserName(model.UserName);
            if (targetAccount != null)
            {
                if(targetAccount.Id != model.Id)
                {
                    return NotFound("Le compte existe deja.");
                }
                
            }

            var account = await accountRepository.GetById(model.Id);
            

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                model.NewPassword = model.NewPassword.Trim();
                model.NewPasswordAgain = model.NewPasswordAgain.Trim();

                if (!model.NewPassword.Equals(model.NewPasswordAgain, StringComparison.CurrentCultureIgnoreCase))
                {
                    return BadRequest("Le mot de passe et le nouveau mot de passe ne correspondent pas.");
                }

                account.PasswordHash = hasher.HashPassword(targetAccount, model.NewPassword);
            }
            account.Name = model.Name;
            account.Username = model.UserName;



            await accountRepository.UpdateAccount(account);


            return Ok(account);
        }

        [HttpDelete]
        [Route("Delete/{accountId}")]
        public async Task<IActionResult> Delete(int accountId)
        {
            var targetAccount = await accountRepository.GetById(accountId);
            if (targetAccount == null)
            {
                return BadRequest("Le compte n'existe pas.");
            }

            await accountRepository.RemoveAccount(accountId);
            return Ok(accountId);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginAccountViewModel model)
        {
            var targetAccount = accountRepository.GetByUserName(model.Username);

            if (targetAccount == null)
            {
                return BadRequest("Le compte n'existe pas.");
            }

            var hasher = new PasswordHasher<Account>();
            var result = hasher.VerifyHashedPassword(targetAccount, targetAccount.PasswordHash, model.Password);

            if (result != PasswordVerificationResult.Success)
            {
                return BadRequest("Mot de passe incorrect.");
            }

            await accountRepository.LogIn(targetAccount);

            LoginAccountViewModel dd = new LoginAccountViewModel();
            dd.Username = model.Username;
            dd.Password = model.Password;

            return Ok(dd);
        }


        [HttpGet]
        [Route("PasswordRecovery/{username}")]
        public IActionResult PasswordRecovery(string username)
        {
            var targetAccount = accountRepository.GetByUserName(username);

            if (targetAccount == null)
            {
                return NotFound();
            }

            string key = RandomString(20);

            sendEmail(targetAccount.Email, key);

            targetAccount.RecoveryCode = key;

            accountRepository.UpdateAccount(targetAccount);
            

            

            //await accountRepository.LogIn(targetAccount);

            //LoginAccountViewModel dd = new LoginAccountViewModel();
            //dd.Username = model.Username;
            //dd.Password = model.Password

            return Ok(targetAccount);
        }

        [HttpPost]
        [Route("PasswordRecoveryPage")]
        public async Task<IActionResult> PasswordRecoveryPage([FromBody]EditAccountViewModel model)
        {
            var hasher = new PasswordHasher<Account>();

            var account = accountRepository.GetByUserRecoveryCode(model.RecoveryCode);
            if(account == null)
            {
                return BadRequest();
            }

            if (!model.NewPassword.Equals(model.NewPasswordAgain, StringComparison.CurrentCultureIgnoreCase))
            {
                return BadRequest("Le mot de passe et le nouveau mot de passe ne correspondent pas.");
            }

            

            account.PasswordHash = hasher.HashPassword(account, model.NewPassword);
            await accountRepository.ResetPassword(account);
            accountRepository.DestroyRecoveryCode(account);

            return Ok(account);

        }



        private bool sendEmail(string email, string key)
        {
            bool status = false;

            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("gestion.comptestest@gmail.com", "Soufiane1234");

            MailMessage mm = new MailMessage("gestion.comptestest@gmail.com", email);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.Subject = "Réenitialisation de mot de passe";
            mm.IsBodyHtml = true;
            mm.Body = "<h2>Réinitialisation de mot de passe</h2><br /><p>Pour réinitialiser votre mot de passe, merci de copier/coller le code ci-dessous</p><br /><h3><strong>"+key+"</strong><h3>";
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
            return status;
        }

    }
}