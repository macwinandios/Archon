using Archon.Shared.Interfaces;
using Archon.ViewModels.Base;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Archon.Views;

namespace Archon.ViewModels
{
    public class AdminAssignTaskViewModel : ViewModelBase, IAdminAssignTaskViewModel
    {
        private int _id;
        private int _numberOfAssignedTasks;
        private string _username;
        private DateTime _dateOfAssignedTask;
        private bool _taskIsComplete;
        private string _taskDescription;
        private string _taskTitle;
        private string _taskCompletedNotes;
        private string _taskWasAssignedTo;

        ICommand _pushToMonitorPayView;
        ICommand _popToLogout;
        ICommand _assignTask;
        ICommand _pushToAdminCompletedTaskDetails;
        ICommand _getAllTasks;
        ICommand _getTaskByIdOrUsername;
        ICommand _updateTask;
        ICommand _deleteTask;
        private readonly IRepository<IAdminAssignTaskViewModel> _iRepository;
        public AdminAssignTaskViewModel() { }
        public AdminAssignTaskViewModel(IRepository<IAdminAssignTaskViewModel> iRepository)
        {
            _iRepository = iRepository;
        }

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public int NumberOfAssignedTasks
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

        public ICommand PushToMonitorPayViewCommand => _pushToMonitorPayView ?? (_pushToMonitorPayView = new Command(PushToMonitorPayView));
        public ICommand PushToAdminCompletedTaskDetailsCommand => _pushToAdminCompletedTaskDetails ?? (_pushToAdminCompletedTaskDetails = new Command(PushToAdminCompletedTaskDetails));
        public ICommand GetAllTasksCommand => _getAllTasks ?? (_getAllTasks = new Command(GetAllTasks));
        public ICommand GetTaskByIdOrUsernameCommand => _getTaskByIdOrUsername ?? (_getTaskByIdOrUsername = new Command(GetTaskByIdOrUsername));

        public ICommand UpdateTaskCommand => _updateTask ?? (_updateTask = new Command(UpdateTask));
        public ICommand DeleteTaskCommand => _deleteTask ?? (_deleteTask = new Command(DeleteTask));
        private async void AssignTask()
        {
            await _iRepository.PostAsync(this);
        }
        private async void GetAllTasks()
        {
            await _iRepository.GetAllAsync(this);

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
        //missing VM
        private async void PushToMonitorPayView(object obj)
        {
            throw new NotImplementedException();
        }
        //missing VM
        private async void PushToAdminCompletedTaskDetails(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
