using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Archon.Shared.Interfaces
{
    public interface IRepository<T> 
    {
        Task PostAsync(T viewModel);
        Task DeleteAsync(T viewModel);
        Task<IEnumerable<T>> GetAllAsync(T viewModel);
        Task GetByIdOrUsername(T viewModel, int id);
        Task GetByIdOrUsername(T viewModel, string username);
        Task PutAsync(T viewModel);
        //Task<bool> Login(ILoginViewModel viewModel);

    }
}
