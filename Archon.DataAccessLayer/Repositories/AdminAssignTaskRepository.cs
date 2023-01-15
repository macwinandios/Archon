using Archon.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Archon.DataAccessLayer.Repositories
{
    public class AdminAssignTaskRepository : IRepository<IAdminAssignTaskViewModel>
    {
        public Task DeleteAsync(IAdminAssignTaskViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IAdminAssignTaskViewModel>> GetAllAsync(IAdminAssignTaskViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task GetByIdOrUsername(IAdminAssignTaskViewModel viewModel, int id)
        {
            throw new NotImplementedException();
        }

        public Task GetByIdOrUsername(IAdminAssignTaskViewModel viewModel, string username)
        {
            throw new NotImplementedException();
        }

        public Task PostAsync(IAdminAssignTaskViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task PutAsync(IAdminAssignTaskViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
