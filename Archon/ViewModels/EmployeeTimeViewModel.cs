using Archon.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;
using Archon.ViewModels.Base;
using Archon.Views;
using System.Collections.ObjectModel;
using Archon.Services;

namespace Archon.ViewModels
{
    public class EmployeeTimeViewModel : ViewModelBase, IEmployeeTimeViewModel
    {
        bool _isClockedIn;
        DateTime _currentTime;
        DateTime _clockedInAt;
        DateTime _clockedOutAt;
        DateTime _dateClockedIn;
        DateTime _dateClockedOut;
        float _hourlyWage;
        float _totalWagesEarnedThisWeek;

        ICommand _clockIn;
        ICommand _clockOut;
        ICommand _getFromHoursAndPayTableAndPushToEmployeeTimeDetailsViewCommand;
        ICommand _getFromAdminAssignTaskTableAndPushToEmployeeTaskCommand;
        ICommand _logoutCommand;

        TimeSpan _durationOfClockIn;
        TimeSpan _totalTimeClockedInToday;
        TimeSpan _totalTimeClockedInThisWeek;
        private IAdminAssignTaskViewModel _adminAssignTaskViewModel;
        private readonly ILoginViewModel _iLoginViewModel;

        private readonly IEmployeeTimeRepository<IEmployeeTimeViewModel> _iEmployeeTimeRepository;
        private readonly IGetAndUpdateAssignedTasksEmployee<IAdminAssignTaskViewModel> _iTaskRepository;
        private readonly IRepository<IEmployeeTimeViewModel> _iRepository;



        public EmployeeTimeViewModel() { }
        public EmployeeTimeViewModel( ILoginViewModel loginViewModel, IEmployeeTimeRepository<IEmployeeTimeViewModel> iEmployeeTimeRepository, IRepository<IEmployeeTimeViewModel> iRepository, IGetAndUpdateAssignedTasksEmployee<IAdminAssignTaskViewModel> iTaskRepository, IAdminAssignTaskViewModel adminAssignTaskViewModel)
        {
            _iLoginViewModel = loginViewModel;
            _iEmployeeTimeRepository = iEmployeeTimeRepository;
            _iRepository = iRepository;
            _iTaskRepository = iTaskRepository;
            _adminAssignTaskViewModel = adminAssignTaskViewModel;
        }
        public ObservableCollection<IEmployeeTimeModel> HoursAndPayCollection { get; set; } = new ObservableCollection<IEmployeeTimeModel>();

        public bool IsClockedIn
        {
            get => _isClockedIn;
            set => SetProperty(ref _isClockedIn, value);
        }
        public float HourlyWage
        {
            get => _hourlyWage;
            set => SetProperty(ref _hourlyWage, value);
        }
        public float TotalWagesEarnedThisWeek
        {
            get => _totalWagesEarnedThisWeek;
            set => SetProperty(ref _totalWagesEarnedThisWeek, value);
        }
        public DateTime CurrentTime
        {
            get => _currentTime;
            set => SetProperty(ref _currentTime, value);
        }
        public DateTime ClockedInAt
        {
            get => _clockedInAt;
            set => SetProperty(ref _clockedInAt, value);
        }
        public DateTime ClockedOutAt
        {
            get => _clockedOutAt;
            set => SetProperty(ref _clockedOutAt, value);
        }
        public DateTime DateClockedIn
        {
            get => _dateClockedIn = ClockedInAt.Date;
            set => SetProperty(ref _dateClockedIn, value);
        }
        public DateTime DateClockedOut
        {
            get => _dateClockedOut = ClockedOutAt.Date;
            set => SetProperty(ref _dateClockedOut, value);
        }
        public TimeSpan DurationOfClockIn
        {
            get => _durationOfClockIn = ClockedOutAt - ClockedInAt;
            set => SetProperty(ref _durationOfClockIn, value);
        }
        public TimeSpan TotalTimeClockedInToday
        {
            get => _totalTimeClockedInToday;
            set => SetProperty(ref _totalTimeClockedInToday, value);
        }
        public TimeSpan TotalTimeClockedInThisWeek
        {
            get
            {
                return _totalTimeClockedInToday;
            }
            set => SetProperty(ref _totalTimeClockedInThisWeek, value);
        }
        public string Username
        {
            get => _iLoginViewModel.Username;
            set
            {
                _iLoginViewModel.Username = value;
                OnPropertyChanged(nameof(Username));
            }
        }


        public ICommand ClockInCommand => _clockIn ?? (_clockIn = new Command(ClockInAsync));
        public ICommand ClockOutCommand => _clockOut ?? (_clockOut = new Command(ClockOutAsync));
        
        public ICommand GetFromHoursAndPayTableAndPushToEmployeeTimeDetailsViewCommand => _getFromHoursAndPayTableAndPushToEmployeeTimeDetailsViewCommand ?? (_getFromHoursAndPayTableAndPushToEmployeeTimeDetailsViewCommand = new Command(GetFromHoursAndPayTableAndPushToEmployeeTimeDetailsView));
        public ICommand GetFromAdminAssignTaskTableAndPushToEmployeeTaskCommand => _getFromAdminAssignTaskTableAndPushToEmployeeTaskCommand ?? (_getFromAdminAssignTaskTableAndPushToEmployeeTaskCommand = new Command(GetFromAdminAssignTaskTableAndPushToEmployeeTaskView));
        public ICommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new Command(LogoutAsync));


        private void ClockInAsync()
        {
            
            try
            {
                _iEmployeeTimeRepository.ClockInAsync(this);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
            }
        }
        private void ClockOutAsync(object obj)
        {
            try
            {
                _iRepository.PostAsync(this);

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
            }
            
        }
        private async void GetFromHoursAndPayTableAndPushToEmployeeTimeDetailsView()
        {
            try
            {
                await _iRepository.GetByIdOrUsername(this, Username);

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
            }
            await Application.Current.MainPage.Navigation.PushAsync(new EmployeeTimeDetailsView());
        }
        private async void GetFromAdminAssignTaskTableAndPushToEmployeeTaskView()
        {
            try
            {
                await _iTaskRepository.GetAssignedTaskEmployee(_adminAssignTaskViewModel, Username);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
            }
            await Application.Current.MainPage.Navigation.PushAsync(new EmployeeTaskView());
        }
        private async void LogoutAsync()
        {
            await Application.Current.MainPage.Navigation.PopAsync();

        }


    }
}
