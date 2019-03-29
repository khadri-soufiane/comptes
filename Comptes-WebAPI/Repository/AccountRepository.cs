using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Comptes_WebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Comptes_WebAPI.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private AccountContext _db;

        public AccountRepository(AccountContext db)
        {
            _db = db;
        }

        public async Task<int> AddAccount(Account account)
        {
            if (_db != null)
            {
                await _db.Accounts.AddAsync(account);
                await _db.SaveChangesAsync();

                return account.Id;
            }

            return 0;

        }

        public async Task<List<Account>> GetAll()
        {
            if (_db != null)
            {
                return await _db.Accounts.ToListAsync();
            }

            return null;

        }

        public async Task<Account> GetById(int? accountId)
        {
            if (_db != null)
            {
                var account = await _db.Accounts.SingleOrDefaultAsync(a => a.Id == accountId);
                return account;
            }

            return null;
        }

        public Account GetByUserName(string userName)
        {
            if (_db != null)
            {
                try
                {
                    var account = _db.Accounts.FirstOrDefault(a => a.Username == userName);
                    if (account != null)
                    {
                        return account;
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }


            }

            return null;
        }

        public async Task LogIn(Account account )
        {
            

            account.LastLoginDateTime = DateTime.Now;
            _db.Update(account);
            await _db.SaveChangesAsync();
        }


        public async Task<int> RemoveAccount(int? accountId)
        {
            int result = 0;

            if (_db != null)
            {
                var account = await _db.Accounts.FindAsync(accountId);

                if (account != null)
                {
                    _db.Accounts.Remove(account);

                    result = await _db.SaveChangesAsync();
                }
            }

            return result;
        }

        public async Task<Account> UpdateAccount(Account account)
        {
            try
            {
                if (_db != null)
                {
                    _db.Accounts.Update(account);

                    await _db.SaveChangesAsync();
                    return account;
                }
            }
            catch (Exception ex)
            {

            }


            return null;


        }
    }
}
