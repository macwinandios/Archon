using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;
namespace Archon.Shared.Interfaces
{
    public interface IPutRepository<T>
    {
        Task PutAsync(T viewModel);
    }
}
