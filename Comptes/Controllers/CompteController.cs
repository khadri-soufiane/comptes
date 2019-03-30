using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Comptes.Models;
using Comptes_WebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Comptes.Controllers
{
    public class CompteController : Controller
    {
        AccountHttpClient _client = new AccountHttpClient();
        

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Account> Accounts = new List<Account>();
            HttpClient client = _client.Initiale();

            HttpResponseMessage res = await client.GetAsync("api/ComptesAPI/GetAccounts");

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                Accounts = JsonConvert.DeserializeObject<List<Account>>(result);
            }

            return View(Accounts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterAccountViewModel model)
        {
            HttpClient client = _client.Initiale();
            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.PostAsJsonAsync("api/ComptesAPI/AddAccount", model);

            if (response.IsSuccessStatusCode)
            {
                LoginAccountViewModel loginModel = new LoginAccountViewModel();
                loginModel.Username = model.Username;
                loginModel.Password = model.Password;
                response = await client.PostAsJsonAsync("api/ComptesAPI/Login", loginModel);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetByUsername(string username)
        {
            HttpClient client = _client.Initiale();
            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.PostAsJsonAsync("api/ComptesAPI/GetByUsername", username);

            if (response.IsSuccessStatusCode)
            {
                
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginAccountViewModel model)
        {
            HttpClient client = _client.Initiale();
            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.PostAsJsonAsync("api/ComptesAPI/Login", model);



            if (response.IsSuccessStatusCode)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, model.Username));


                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                try
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                }
                catch (Exception ex)
                {

                }
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string username)
        {
            HttpClient client = _client.Initiale();
            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.GetAsync("api/ComptesAPI/GetByUsername/"+username);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var account =  JsonConvert.DeserializeObject<Account>(data);
                return View(account);
            }
            return BadRequest();
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            HttpClient client = _client.Initiale();
            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.GetAsync("api/ComptesAPI/GetById/" + id);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var account = JsonConvert.DeserializeObject<Account>(data);
                return View(account);
            }
            return BadRequest();

        }

    }
}