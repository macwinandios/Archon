
using Archon.DataAccessLayer;
using Archon.Shared.Interfaces;
using Archon.ViewModels.Base;
using Archon.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Archon.ViewModels
{
    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        int? _id;
        int? _companyId;
        string _username;
        string _password;
        ICommand _signUpCommand;
        ICommand _getUsersCommand;
        ICommand _getUsersByIdCommand;
        ICommand _deleteUserCommand;
        ICommand _updateUserCommand;
        ICommand _loginCommand;
        bool _getUserByIdIsVisible;
        bool _managerButtons;
        private readonly IRepository<ILoginViewModel> _iRepository;
        private readonly ILoginRepository<ILoginViewModel> _iLoginRepository;
        public LoginViewModel() { }
        public LoginViewModel(IRepository<ILoginViewModel> iRepository,ILoginRepository<ILoginViewModel> iLoginRepository)
        {
            _iRepository = iRepository;
            _iLoginRepository = iLoginRepository;

        }
        public bool ManagerButtons
        {
            get => _managerButtons;
            set => SetProperty(ref _managerButtons, value);
        }
        public bool GetUserByIdIsVisible
        {
            get => _getUserByIdIsVisible;
            set => SetProperty(ref _getUserByIdIsVisible, value);
        }
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }
        public int? CompanyId
        {
            get => _companyId;
            set
            {
                _companyId = value;
                OnPropertyChanged(nameof(CompanyId));
                if (CompanyId == 999)
                {
                    ManagerButtons = true;
                }
                else ManagerButtons = false;
            }
        }
        public int? Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public ICommand SignUpCommand => _signUpCommand ?? (_signUpCommand = new Command(SignUpAsync));
        public ICommand DeleteCommand => _deleteUserCommand ?? (_deleteUserCommand = new Command(DeleteUserAsync));
        public ICommand GetUsersCommand => _getUsersCommand ?? (_getUsersCommand = new Command(GetAllUsersAsync));
        public ICommand GetUsersByIdCommand => _getUsersByIdCommand ?? (_getUsersByIdCommand = new Command(GetUsersByIdAsync));
        public ICommand UpdateCommand => _updateUserCommand ?? (_updateUserCommand = new Command(UpdateUserAsync));
        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new Command(LoginAsync));

        public async void LoginAsync()
        {

           // if (await _iRepository.Login(this))
            if (await _iLoginRepository.Login(this))

                {
                    await Application.Current.MainPage.Navigation.PushAsync(new EmployeeTimeView());
            }
        }
        //public void LoginAsync()
        //{
        //    Application.Current.MainPage.Navigation.PushAsync(new EmployeeTimeDetailsView());

        //}
        public async void SignUpAsync()
        {
            GetUserByIdIsVisible = false;
            await _iRepository.PostAsync(this);
        }
        public async void GetAllUsersAsync()
        {
            GetUserByIdIsVisible = false;
            try
            {
                await _iRepository.GetAllAsync(this);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
            }
        }
        public async void GetUsersByIdAsync()
        {
            GetUserByIdIsVisible = true;
            await _iRepository.GetByIdOrUsername(this, (int)Id);

        }
        public async void DeleteUserAsync()
        {
            GetUserByIdIsVisible = false;
            await _iRepository.DeleteAsync(this);
        }
        public async void UpdateUserAsync()
        {
            GetUserByIdIsVisible = false;
            await _iRepository.PutAsync(this);
        }

        public ObservableCollection<ILoginModel> UserList { get; set; } = new ObservableCollection<ILoginModel>();
    }
}
