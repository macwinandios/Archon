using Archon.Shared.Interfaces;
using Archon.ViewModels.Base;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Archon.Views;
using System.Collections.ObjectModel;

namespace Archon.ViewModels
{
    public class AdminAssignTaskViewModel : ViewModelBase, IAdminAssignTaskViewModel
    {
        private int? _id;
        private int? _numberOfAssignedTasks;
        private string _username;
        private DateTime _dateOfAssignedTask;
        private bool _taskIsComplete;
        private string _taskDescription;
        private string _taskTitle;
        private string _taskCompletedNotes;
        private string _taskWasAssignedTo;

        ICommand _pushToAdminMonitorPayViewCommand;
        ICommand _popToLogout;
        ICommand _assignTask;
        ICommand _getValuesFromAssignedTaskTableAndPushToAdminCompletedTaskViewCommand;
        ICommand _employeeUpdateAssignedTaskTableCommand;

        ICommand _getTaskByIdOrUsername;
        ICommand _updateTask;
        ICommand _deleteTask;
        private readonly IRepository<IAdminAssignTaskViewModel> _iRepository;
        private readonly IGetAndUpdateAssignedTasksEmployee<IAdminAssignTaskViewModel> _iTaskRepository;
        public AdminAssignTaskViewModel() { }
        public AdminAssignTaskViewModel(IRepository<IAdminAssignTaskViewModel> iRepository, IGetAndUpdateAssignedTasksEmployee<IAdminAssignTaskViewModel> iTaskRepository)
        {
            _iRepository = iRepository;
            _iTaskRepository = iTaskRepository;
        }
        public ObservableCollection<IAdminAssignTaskModel> TaskCollection { get; set; } = new ObservableCollection<IAdminAssignTaskModel>();
        public int? Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public int? NumberOfAssignedTasks
        {
            get => _numberOfAssignedTasks;
            set => SetProperty(ref _numberOfAssignedTasks, value);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }


        public DateTime DateOfAssignedTask
        {
            get => _dateOfAssignedTask;
            set => SetProperty(ref _dateOfAssignedTask, value);
        }

        public bool TaskIsComplete
        {
            get => _taskIsComplete;
            set => SetProperty(ref _taskIsComplete, value);
        }


        public string TaskDescription
        {
            get => _taskDescription;
            set => SetProperty(ref _taskDescription, value);
        }

        public string TaskTitle
        {
            get => _taskTitle;
            set => SetProperty(ref _taskTitle, value);
        }


        public string TaskCompletedNotes
        {
            get => _taskCompletedNotes;
            set => SetProperty(ref _taskCompletedNotes, value);
        }


        public string TaskWasAssignedTo
        {
            get => _taskWasAssignedTo;
            set => SetProperty(ref _taskWasAssignedTo, value);
        }


        public ICommand AssignTaskCommand => _assignTask ?? (_assignTask = new Command(AssignTask));
        public ICommand PopToLogoutCommand => _popToLogout ?? (_popToLogout = new Command(PopToLogout));

        public ICommand PushToAdminMonitorPayViewCommand => _pushToAdminMonitorPayViewCommand ?? (_pushToAdminMonitorPayViewCommand = new Command(PushToAdminMonitorPayView));
        public ICommand GetValuesFromAssignedTaskTableAndPushToAdminCompletedTaskViewCommand => _getValuesFromAssignedTaskTableAndPushToAdminCompletedTaskViewCommand ?? (_getValuesFromAssignedTaskTableAndPushToAdminCompletedTaskViewCommand = new Command(GetAllValuesFromAssignedTaskTableAndPushToAdminCompletedTaskView));
        public ICommand GetTaskByIdOrUsernameCommand => _getTaskByIdOrUsername ?? (_getTaskByIdOrUsername = new Command(GetTaskByIdOrUsername));

        public ICommand UpdateTaskCommand => _updateTask ?? (_updateTask = new Command(UpdateTask));
        public ICommand EmployeeUpdateAssignedTaskTableCommand => _employeeUpdateAssignedTaskTableCommand ?? (_employeeUpdateAssignedTaskTableCommand = new Command(EmployeeUpdateAssignedTaskTable));
        public ICommand DeleteTaskCommand => _deleteTask ?? (_deleteTask = new Command(DeleteTask));

        private async void EmployeeUpdateAssignedTaskTable()
        {
            try
            {
                await _iTaskRepository.UpdateAssignedTaskEmployee(this, (int)Id);

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
            }
        }

        private async void AssignTask()
        {
            await _iRepository.PostAsync(this);
            NumberOfAssignedTasks++;
        }
        private async void GetTaskByIdOrUsername()
        {
            await _iRepository.GetByIdOrUsername(this, Username);
        }
        private async void UpdateTask()
        {
            await _iRepository.PutAsync(this);

        }
        private async void DeleteTask()
        {
            await _iRepository.DeleteAsync(this);

        }
        private async void PopToLogout(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
        }
        private async void PushToAdminMonitorPayView(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AdminMonitorPayView());
        }
        private async void GetAllValuesFromAssignedTaskTableAndPushToAdminCompletedTaskView()
        {
            try
            {
                await _iRepository.GetAllAsync(this);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
            }
            await Application.Current.MainPage.Navigation.PushAsync(new AdminCompletedTaskView());
        }
    }
}
