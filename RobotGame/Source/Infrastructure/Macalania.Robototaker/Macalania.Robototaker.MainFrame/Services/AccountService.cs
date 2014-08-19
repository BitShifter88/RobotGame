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
        public void CreateAccount(string username)
        {
            AccountRepository ar = new AccountRepository();
            Account ac = new Account() { Username = username };
            ar.Add(ac);
        }
    }
}
