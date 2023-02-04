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
            _container.Register<IRepository<ILoginViewModel>, LoginRepository>();
            _container.Register<ILoginRepository<ILoginViewModel>, LoginRepository >();

            //EMPLOYEE TIME REPOSITORY
            _container.Register<IEmployeeTimeRepository<IEmployeeTimeViewModel>, EmployeeTimeRepository>();
            _container.Register<IRepository<IEmployeeTimeViewModel>, EmployeeTimeRepository>();

            //ADMINASSIGNTASK REPOSITORY
            _container.Register<IRepository<IAdminAssignTaskViewModel>, AdminAssignTaskRepository>();
                _container.Register<IGetAndUpdateAssignedTasksEmployee<IAdminAssignTaskViewModel>, AdminAssignTaskRepository>();

            //EMPLOYEE TIME VIEWMODEL
            _container.Register<IEmployeeTimeViewModel, EmployeeTimeViewModel>();


            //LOGIN VIEWMODEL
            _container.Register<ILoginViewModel, LoginViewModel>();


            //ADMIN ASSIGN TASK VIEWMODEL
            _container.Register<IAdminAssignTaskViewModel, AdminAssignTaskViewModel>();

        }
        public ILoginViewModel LoginViewModel => _container.Resolve<ILoginViewModel>();
        public IEmployeeTimeViewModel EmployeeTimeViewModel => _container.Resolve<IEmployeeTimeViewModel>();
        public IAdminAssignTaskViewModel AdminAssignTaskViewModel => _container.Resolve<IAdminAssignTaskViewModel>();

    }
}
