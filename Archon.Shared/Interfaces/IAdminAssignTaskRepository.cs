using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Archon.Shared.Interfaces
{
    public interface IAdminAssignTaskRepository<T>
    {
        Task GetAssignedTaskEmployee(T viewModel, string username);
    }
}
