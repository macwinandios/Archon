using Archon.Shared.Interfaces;
using Archon.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Archon.ViewModels
{
    public class AdminMonitorPayViewModel : ViewModelBase, IAdminMonitorPayViewModel
    {
        bool _isClockedIn;
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
        string _username;

        public readonly IEmployeeTimeViewModel _iEmployeeTimeViewModel;

        //i may need the username from loginvm MAYBE
        private readonly IGetRepository<IEmployeeTimeViewModel> _iGetRepository;
        private readonly IPutRepository<IEmployeeTimeViewModel> _iPutRepository;
        private readonly IDeleteRepository<IEmployeeTimeViewModel> _iDeleteRepository;


        public AdminMonitorPayViewModel() { }
        public AdminMonitorPayViewModel( IGetRepository<IEmployeeTimeViewModel> iGetRepository, IPutRepository<IEmployeeTimeViewModel> iPutRepository, IDeleteRepository<IEmployeeTimeViewModel> iDeleteRepository, IEmployeeTimeViewModel iEmployeeViewModel)
        {
            _iGetRepository = iGetRepository;
            _iPutRepository = iPutRepository;
            _iDeleteRepository = iDeleteRepository;
            _iEmployeeTimeViewModel = iEmployeeViewModel;
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
            //get => _username;
            //set => SetProperty(ref _username, value);
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
            await _iDeleteRepository.DeleteAsync(_iEmployeeTimeViewModel);

        }
        private async void UpdateTimeDetail()
        {
            await _iPutRepository.PutAsync(_iEmployeeTimeViewModel);

        }
        private async void GetAllTimeDetailsForUser()
        {
            await _iGetRepository.GetByIdOrUsername(_iEmployeeTimeViewModel, Username);
        }
    }
}
