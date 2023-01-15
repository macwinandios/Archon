using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Archon.Shared.Interfaces
{
    public interface ILoginModel
    {
        int Id { get; set; }
        string Username { get; set; }
        string Password { get; set; }

        ObservableCollection<ILoginModel> UserList { get; set; }

    }
}
