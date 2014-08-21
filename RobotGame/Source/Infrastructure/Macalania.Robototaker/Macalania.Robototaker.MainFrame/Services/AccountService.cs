using Macalania.Robototaker.MainFrame.Data.Mapping;
using Macalania.Robototaker.MainFrame.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Services
{
    class AccountService
    {
        AccountRepository _ar;

        public AccountService()
        {
            _ar = new AccountRepository();
        }

        public bool CreateAccount(string username, string inGameName, string password)
        {
            Account ac = new Account() { Username = username, IngameName = inGameName };

            ac.PasswordSalt = Hash.CreateSalt();
            ac.PasswordHash = Hash.CreatePasswordHash(password, ac.PasswordSalt);

            if (DoesAccountExist(username) == false)
                _ar.Add(ac);
            else return false;
            return true;
        }

        public Account IsValidLogin(string username, string password)
        {
            Account account = _ar.GetAccount(username);

            if (account == null)
                return null;

            string passHash = Hash.CreatePasswordHash(password, account.PasswordSalt);

            if (account.PasswordHash == passHash)
                return account;
            return null;
        }

        public bool DoesAccountExist(string username)
        {
            if (_ar.GetAccount(username) == null)
                return false;
            else return true;
        }
    }
}
