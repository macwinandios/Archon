using Archon.Shared.Interfaces;
using Archon.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Archon.ViewModels
{
    public class AdminMonitorPayViewModel : ViewModelBase, IAdminMonitorPayViewModel
    {
        bool _isClockedIn;
        bool _userDetailsIsVisible;
        float _hourlyWage;
        float _wagesEarned;
        DateTime _currentTime;
        DateTime _clockedInAt;
        DateTime _clockedOutAt;
        DateTime _dateClockedIn;
        DateTime _dateClockedOut;
        TimeSpan _durationOfClockIn;
        TimeSpan _totalTimeClockedInToday;
        TimeSpan _totalTimeClockedInThisWeek;
        ICommand _getAllTimeDetailsForUserCommand;
        ICommand _updateTimeDetailCommand;
        ICommand _deleteTimeDetailCommand;

        public readonly IEmployeeTimeViewModel _iEmployeeTimeViewModel;

        private readonly IRepository<IEmployeeTimeViewModel> _iRepository;

        public AdminMonitorPayViewModel() { }
        public AdminMonitorPayViewModel(IEmployeeTimeViewModel iEmployeeViewModel, IRepository<IEmployeeTimeViewModel> iRepository)
        {
            _iEmployeeTimeViewModel = iEmployeeViewModel;
            _iRepository = iRepository;
        }
        public ObservableCollection<IEmployeeTimeModel> HoursAndPayCollection
        {
            get => _iEmployeeTimeViewModel.HoursAndPayCollection;
            set
            {
                _iEmployeeTimeViewModel.HoursAndPayCollection = value;
                OnPropertyChanged(nameof(HoursAndPayCollection));
            }
        }
        public bool UserDetailsIsVisible
        {
            get => _userDetailsIsVisible;
            set => SetProperty(ref _userDetailsIsVisible, value);
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
            get => _dateClockedOut;
            set => SetProperty(ref _dateClockedOut, value);
        }
        public TimeSpan DurationOfClockIn
        {
            get => _durationOfClockIn;
            set => SetProperty(ref _durationOfClockIn, value);
        }
        public TimeSpan TotalTimeClockedInToday
        {
            get => _totalTimeClockedInToday;
            set => SetProperty(ref _totalTimeClockedInToday, value);
        }
        public TimeSpan TotalTimeClockedInThisWeek
        {
            get => _totalTimeClockedInThisWeek;
            set => SetProperty(ref _totalTimeClockedInThisWeek, value);
        }

        public string Username
        {
            get => _iEmployeeTimeViewModel.Username;
            set
            {
                _iEmployeeTimeViewModel.Username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public ICommand GetAllTimeDetailsForUserCommand => _getAllTimeDetailsForUserCommand ?? (_getAllTimeDetailsForUserCommand = new Command(GetAllTimeDetailsForUser));

        public ICommand UpdateTimeDetailCommand => _updateTimeDetailCommand ?? (_updateTimeDetailCommand = new Command(UpdateTimeDetail));

        public ICommand DeleteTimeDetailCommand => _deleteTimeDetailCommand ?? (_deleteTimeDetailCommand = new Command(DeleteTimeDetail));

        private async void DeleteTimeDetail()
        {
            await _iRepository.DeleteAsync(_iEmployeeTimeViewModel);

        }
        private async void UpdateTimeDetail()
        {
            await _iRepository.PutAsync(_iEmployeeTimeViewModel);

        }
        private async void GetAllTimeDetailsForUser()
        {
            await _iRepository.GetByIdOrUsername(_iEmployeeTimeViewModel, Username);
            UserDetailsIsVisible = true;
        }
    }
}
