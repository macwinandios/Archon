using Archon.DataAccessLayer;
using Archon.DataAccessLayer.Repositories;
using Archon.Shared.Interfaces;
using Archon.ViewModels;
using Archon.Views;
using System;
using System.Collections.Generic;
using System.Text;
using TinyIoC;

namespace Archon.Services
{
    public class ViewModelLocator
    {
        private static readonly TinyIoCContainer _container;
        static ViewModelLocator()
        {
            _container = new TinyIoCContainer();

            //LOGIN REPOSITORY
            _container.Register<IRepository<ILoginModel>, LoginRepository>();
            _container.Register<ILoginRepository<ILoginModel>, LoginRepository>();

            //EMPLOYEE TIME REPOSITORY
            _container.Register<IEmployeeTimeRepository<IEmployeeTimeModel>, EmployeeTimeRepository>();
            _container.Register<IRepository<IEmployeeTimeModel>, EmployeeTimeRepository>();

            //ADMINASSIGNTASK REPOSITORY
            _container.Register<IRepository<IAdminAssignTaskModel>, AdminAssignTaskRepository>();
                _container.Register<IGetAndUpdateAssignedTasksEmployee<IAdminAssignTaskModel>, AdminAssignTaskRepository>();

            //EMPLOYEETIME VIEWMODEL
            _container.Register<IEmployeeTimeModel, EmployeeTimeViewModel>();

            //LOGIN VIEWMODEL
            _container.Register<ILoginModel, LoginViewModel>();

            //ADMIN ASSIGN TASK VIEWMODEL
            _container.Register<IAdminAssignTaskModel, AdminAssignTaskViewModel>();

        }
        public ILoginModel LoginViewModel => _container.Resolve<ILoginModel>();
        public IEmployeeTimeModel EmployeeTimeViewModel => _container.Resolve<IEmployeeTimeModel>();
        public IAdminAssignTaskModel AdminAssignTaskViewModel => _container.Resolve<IAdminAssignTaskModel>();

    }
}
