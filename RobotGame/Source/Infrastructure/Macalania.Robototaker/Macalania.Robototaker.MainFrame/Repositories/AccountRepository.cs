using Macalania.Robototaker.MainFrame.Data;
using Macalania.Robototaker.MainFrame.Data.Mapping;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Repositories
{
    class AccountRepository
    {
        public void Add(Account product)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(product);
                transaction.Commit();
            }
        }

        public Account GetAccount(string username)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
               Account account = session.CreateCriteria(typeof(Account)).
                                Add(Restrictions.Eq("Username", username)).
                                UniqueResult<Account>();

               return account;
            }
        }
    }
}
