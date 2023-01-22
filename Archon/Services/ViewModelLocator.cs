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
            _container.Register<IPostRepository<IEmployeeTimeViewModel>, EmployeeTimeRepository>();
            _container.Register<IGetRepository<IEmployeeTimeViewModel>, EmployeeTimeRepository>();

            //ADMINASSIGNTASK REPOSITORY
            _container.Register<IRepository<IAdminAssignTaskViewModel>, AdminAssignTaskRepository>();

            //ADMIN MONITOR PAY REPOTISORY



            //EMPLOYEE TIME VIEWMODEL
            _container.Register<IEmployeeTimeViewModel, EmployeeTimeViewModel>();


            //LOGIN VIEWMODEL
            _container.Register<ILoginViewModel, LoginViewModel>();


            //ADMIN ASSIGN TASK VIEWMODEL
            _container.Register<IAdminAssignTaskViewModel, AdminAssignTaskViewModel>();


            //ADMIN MONITOR PAY VIEW MODEL
            _container.Register<IAdminMonitorPayViewModel, AdminMonitorPayViewModel>();


        }
        public ILoginViewModel LoginViewModel => _container.Resolve<ILoginViewModel>();
        public IEmployeeTimeViewModel EmployeeTimeViewModel => _container.Resolve<IEmployeeTimeViewModel>();
        public IAdminAssignTaskViewModel AdminAssignTaskViewModel => _container.Resolve<IAdminAssignTaskViewModel>();
        public IAdminMonitorPayViewModel AdminMonitorPayViewModel => _container.Resolve<IAdminMonitorPayViewModel>();

    }
}
