using Archon.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Archon.DataAccessLayer
{
    public class LoginModel : ILoginModel
    {
        public int? Id { get; set; }
        public int? CompanyId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ObservableCollection<ILoginModel> UserList { get; set; }

    }
}
