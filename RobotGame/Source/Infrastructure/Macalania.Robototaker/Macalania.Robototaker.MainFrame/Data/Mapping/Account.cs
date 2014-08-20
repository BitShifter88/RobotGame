using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.MainFrame.Data.Mapping
{
    class Account
    {
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string PasswordSalt { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string IngameName { get; set; }
    }
}
