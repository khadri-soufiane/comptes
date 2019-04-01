using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Comptes.Models
{
    public class AccountHttpClient
    {
        public HttpClient Initiale()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://comptes-api.azurewebsites.net/");
            return client;
        }
    }
}
