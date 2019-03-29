using Comptes_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comptes_WebAPI.Repository
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAll();

        Task<Account> GetById(int? accountId);

        Account GetByUserName(string userName);

        Task<int> AddAccount(Account account);

        Task<Account> UpdateAccount(Account account);

        Task<int> RemoveAccount(int? accountId);

        Task LogIn(Account account) ;

    }
}
