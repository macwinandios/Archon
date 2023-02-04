using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Archon.Shared.Interfaces
{
    public interface IGetAndUpdateAssignedTasksEmployee<T>
    {
        Task GetAssignedTaskEmployee(T viewModel, string username);
        Task UpdateAssignedTaskEmployee(T viewModel, int id);
    }
}
