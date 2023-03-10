using Archon.Shared.Interfaces;
using Archon.ViewModels.Base;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Archon.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Archon.ViewModels
{
    public class AdminAssignTaskViewModel : ViewModelBase, IAdminAssignTaskModel
    {
        int? _id;
        int? _numberOfAssignedTasks;
        bool _taskIsComplete;
        DateTime _dateOfAssignedTask;
        string _taskDescription;
        string _taskTitle;
        string _taskCompletedNotes;
        string _taskWasAssignedTo;
        string _username;

        ICommand _pushToAdminMonitorPayViewCommand;
        ICommand _popToAdminAssignTaskViewCommand;
        ICommand _logoutCommand;
        ICommand _assignTaskCommand;
        ICommand _getValuesFromAssignedTaskTableAndPushToAdminCompletedTaskViewCommand;
        ICommand _employeeUpdateAssignedTaskTableCommand;
        ICommand _getTaskByIdOrUsernameCommand;
        ICommand _updateTaskCommand;
        ICommand _deleteTaskCommand;

        private readonly IRepository<IAdminAssignTaskModel> _iRepository;
        private readonly IGetAndUpdateAssignedTasksEmployee<IAdminAssignTaskModel> _iTaskRepository;
        public AdminAssignTaskViewModel() { }
        public AdminAssignTaskViewModel(IRepository<IAdminAssignTaskModel> iRepository, IGetAndUpdateAssignedTasksEmployee<IAdminAssignTaskModel> iTaskRepository)
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


        public ICommand AssignTaskCommand => _assignTaskCommand ?? (_assignTaskCommand = new Command(async () => await AssignTask()));
        public ICommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new Command(async () => await PopToLogin()));

        public ICommand PushToAdminMonitorPayViewCommand => _pushToAdminMonitorPayViewCommand ?? (_pushToAdminMonitorPayViewCommand = new Command(async () => await PushToAdminMonitorPayView()));
        public ICommand PopToAdminAssignTaskViewCommand => _popToAdminAssignTaskViewCommand ?? (_popToAdminAssignTaskViewCommand = new Command(async () => await PopToAdminAssignTaskView()));
        public ICommand GetValuesFromAssignedTaskTableAndPushToAdminCompletedTaskViewCommand => _getValuesFromAssignedTaskTableAndPushToAdminCompletedTaskViewCommand ?? (_getValuesFromAssignedTaskTableAndPushToAdminCompletedTaskViewCommand = new Command(async () => await GetAllValuesFromAssignedTaskTableAndPushToAdminCompletedTaskView()));
        public ICommand GetTaskByIdOrUsernameCommand => _getTaskByIdOrUsernameCommand ?? (_getTaskByIdOrUsernameCommand = new Command(async () => await GetTaskByIdOrUsername()));

        public ICommand UpdateTaskCommand => _updateTaskCommand ?? (_updateTaskCommand = new Command(async () => await UpdateTask()));
        public ICommand EmployeeUpdateAssignedTaskTableCommand => _employeeUpdateAssignedTaskTableCommand ?? (_employeeUpdateAssignedTaskTableCommand = new Command(async () => await EmployeeUpdateAssignedTaskTable()));
        public ICommand DeleteTaskCommand => _deleteTaskCommand ?? (_deleteTaskCommand = new Command(async () => await DeleteTask()));

        private async Task EmployeeUpdateAssignedTaskTable()
        {
            try
            {
                if (Id == null)
                {
                    await Application.Current.MainPage.DisplayAlert("viewmodel YOU MUST ENTER A VALID ID", "TRY AGAIN", "OK");
                    return;
                }
                await _iTaskRepository.UpdateAssignedTaskEmployee(this, (int)Id);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
            }
        }

        private async Task AssignTask()
        {
            await _iRepository.PostAsync(this);
            NumberOfAssignedTasks++;
        }
        private async Task GetTaskByIdOrUsername()
        {
            await _iRepository.GetByIdOrUsername(this, TaskWasAssignedTo);
            await Application.Current.MainPage.Navigation.PushAsync(new AdminCompletedTaskView());
        }
        private async Task UpdateTask()
        {
            await _iRepository.PutAsync(this);
        }
        private async Task DeleteTask()
        {
            await _iRepository.DeleteAsync(this);
        }
        private async Task PopToLogin()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        private async Task PushToAdminMonitorPayView()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AdminMonitorPayView());
        }
        private async Task PopToAdminAssignTaskView()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        private async Task GetAllValuesFromAssignedTaskTableAndPushToAdminCompletedTaskView()
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
