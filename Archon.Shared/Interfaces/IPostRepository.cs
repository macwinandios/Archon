using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Archon.Shared.Interfaces
{
    public interface IPostRepository<T>
    {
        Task PostAsync(T viewModel);
    }
}
