using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Archon.Shared.Interfaces
{
    public interface IGetRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(T viewModel);
        Task GetByIdOrUsername(T viewModel, int id);
        Task GetByIdOrUsername(T viewModel, string username);
    }
}
