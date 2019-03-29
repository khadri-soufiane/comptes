using System;
using System.Collections.Generic;
using System.Linq;
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

                if (accountId == null)
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
        [Route("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromBody]EditAccountViewModel model)
        {
            var hasher = new PasswordHasher<Account>();

            if (!ModelState.IsValid)
            {
                return BadRequest("Informations de compte non valides.");
            }

            var targetAccount = accountRepository.GetByUserName(model.UserName);
            if (targetAccount == null)
            {
                return BadRequest("Le compte n'existe pas.");
            }

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                model.NewPassword = model.NewPassword.Trim();
                model.NewPasswordAgain = model.NewPasswordAgain.Trim();

                if (!model.NewPassword.Equals(model.NewPasswordAgain, StringComparison.CurrentCultureIgnoreCase))
                {
                    return BadRequest("Le mot de passe et le nouveau mot de passe ne correspondent pas.");
                }

                targetAccount.PasswordHash = hasher.HashPassword(targetAccount, model.NewPassword);
            }
            targetAccount.Name = model.Name;

            await accountRepository.UpdateAccount(targetAccount);


            return Ok(targetAccount);
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
        public async Task<IActionResult> Login(LoginAccountViewModel model)
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
            HttpContext httpcontext = this.HttpContext;

            await accountRepository.LogIn(targetAccount, httpcontext);
            return Ok("Login OK");
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await accountRepository.LogOut(this.HttpContext);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("PasswordRecovery")]
        public async Task<IActionResult> PasswordRecovery([FromHeader]string Username)
        {
            return Ok();
        }
    }
}