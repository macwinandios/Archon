using Archon.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;
using Archon.ViewModels.Base;
using Archon.Views;
using System.Collections.ObjectModel;

namespace Archon.ViewModels
{
    public class EmployeeTimeViewModel : ViewModelBase, IEmployeeTimeViewModel
    {
        bool _isClockedIn;
        ICommand _clockIn;
        ICommand _clockOut;
        DateTime _currentTime;
        DateTime _clockedInAt;
        DateTime _clockedOutAt;
        DateTime _dateClockedIn;
        DateTime _dateClockedOut;
        float _hourlyWage;
        float _wagesEarned;
        TimeSpan _durationOfClockIn;
        ICommand _goToEmployeeDetailsPage;
        ICommand _logoutCommand;
        TimeSpan _totalTimeClockedInToday;
        TimeSpan _totalTimeClockedInThisWeek;
        private readonly ILoginViewModel _iLoginViewModel;
        private readonly IEmployeeTimeRepository<IEmployeeTimeViewModel> _iEmployeeTimeRepository;
        private readonly IPostRepository<IEmployeeTimeViewModel> _iPostRepository;
        public EmployeeTimeViewModel() { }
        public EmployeeTimeViewModel( ILoginViewModel loginViewModel, IEmployeeTimeRepository<IEmployeeTimeViewModel> iEmployeeTimeRepository,IPostRepository<IEmployeeTimeViewModel> iPostRepository)
        {
            _iLoginViewModel = loginViewModel;
            _iEmployeeTimeRepository = iEmployeeTimeRepository;
            _iPostRepository = iPostRepository;
        }


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
        public float WagesEarned
        {
            get => _wagesEarned;
            set => SetProperty(ref _wagesEarned, value);
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
            get => _dateClockedIn;
            set => SetProperty(ref _dateClockedIn, value);
        }
        public DateTime DateClockedOut
        {
            get => _dateClockedOut = DateTime.Today;
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
        
        private static DateTime GetStartOfWeek(DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            return date.AddDays(-dayOfWeek).Date;
        }
        public TimeSpan TotalTimeClockedInThisWeek
        {
            get
            {
                var currentWeekStart = GetStartOfWeek(DateTime.Now);
                var currentWeekEnd = currentWeekStart.AddDays(7);
                if (ClockedOutAt > currentWeekStart && ClockedInAt < currentWeekEnd)
                {
                    if (ClockedOutAt > currentWeekEnd)
                    {
                        _totalTimeClockedInThisWeek += currentWeekEnd - ClockedInAt;
                    }
                    else
                    {
                        _totalTimeClockedInThisWeek += ClockedOutAt - ClockedInAt;
                    }
                }
                return _totalTimeClockedInThisWeek;

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
        public ICommand GoToEmployeeDetailsPageCommand => _goToEmployeeDetailsPage ?? (_goToEmployeeDetailsPage = new Command(GoToEmployeeDetailsPageAsync));
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
                _iPostRepository.PostAsync(this);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
            }
            
        }
        private void GoToEmployeeDetailsPageAsync()
        {
            Application.Current.MainPage.Navigation.PushAsync(new EmployeeTimeDetailsView());
        }
        private void LogoutAsync()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }


    }
}
