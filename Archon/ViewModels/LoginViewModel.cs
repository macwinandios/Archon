
using Archon.DataAccessLayer;
using Archon.Services;
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
    
    public class LoginViewModel : ViewModelBase, ILoginModel
    {

        int? _id;
        int? _companyId;
        string _username;
        string _password;
        bool _managerButtons;

        ICommand _signUpCommand;
        ICommand _getUsersCommand;
        ICommand _getUsersByIdCommand;
        ICommand _getUsersByUsernameCommand;
        ICommand _deleteUserCommand;
        ICommand _updateUserCommand;
        ICommand _loginCommand;
        ICommand _pushToLoginDetailsViewCommand;
        ICommand _popToLoginViewCommand;

        private readonly IRepository<ILoginModel> _iRepository;
        private readonly ILoginRepository<ILoginModel> _iLoginRepository;

        public LoginViewModel() { }
        public LoginViewModel(IRepository<ILoginModel> iRepository,ILoginRepository<ILoginModel> iLoginRepository)
        {
            _iRepository = iRepository;
            _iLoginRepository = iLoginRepository;
        }
        public ObservableCollection<ILoginModel> UserList { get; set; } = new ObservableCollection<ILoginModel>();
        public bool ManagerButtons
        {
            get => _managerButtons;
            set => SetProperty(ref _managerButtons, value);
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
        public ICommand SignUpCommand => _signUpCommand ?? (_signUpCommand = new Command(async () => await SignUpAsync()));
        public ICommand DeleteCommand => _deleteUserCommand ?? (_deleteUserCommand = new Command(async () => await DeleteUserAsync()));
        public ICommand GetUsersCommand => _getUsersCommand ?? (_getUsersCommand = new Command(async () => await GetAllUsersAsync()));
        public ICommand GetUsersByIdCommand => _getUsersByIdCommand ?? (_getUsersByIdCommand = new Command(async () => await GetUsersByIdAsync()));
        public ICommand GetUsersByUsernameCommand => _getUsersByUsernameCommand ?? (_getUsersByUsernameCommand = new Command(async () => await GetUsersByUsernameAsync()));
        public ICommand UpdateCommand => _updateUserCommand ?? (_updateUserCommand = new Command(async () => await UpdateUserAsync()));
        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new Command(async () => await LoginAsync()));
        public ICommand PushToLoginDetailsViewCommand => _pushToLoginDetailsViewCommand ?? (_pushToLoginDetailsViewCommand = new Command(async () => await PushToLoginDetailsViewAsync()));
        public ICommand PopToLoginViewCommand => _popToLoginViewCommand ?? (_popToLoginViewCommand = new Command(async () => await PopToLoginViewAsync()));

        private async Task PopToLoginViewAsync()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
            Username = String.Empty;
            Password = String.Empty;
            CompanyId = null;
        }

        private async Task PushToLoginDetailsViewAsync()
        {
            if (await _iLoginRepository.Login(this))
            {
                if (CompanyId == 999)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginDetailsView());

                    Username = String.Empty;
                }
            }
        }
        private async Task LoginAsync()
        {

            if (await _iLoginRepository.Login(this))

            {
                if (CompanyId == 123)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new EmployeeTimeView());
                }
                if (CompanyId == 999)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new AdminAssignTaskView());
                    Username = String.Empty;
                }
            }
        }

        private async Task SignUpAsync()
        {
            await _iRepository.PostAsync(this);
        }
        private async Task GetAllUsersAsync()
        {
            try
            {
                await _iRepository.GetAllAsync(this);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("VIEWMODEL EXCEPTION", ex.Message, "OK");
            }
        }

        private async Task GetUsersByIdAsync()
        {
            try
            {
                if (Id == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Must Enter a Valid Id", "Try Again", "OK");
                    return;
                }
                await _iRepository.GetByIdOrUsername(this, (int)Id);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("VIEWMODEL EXCEPTION", ex.Message, "OK");
            }
        }

        public async Task GetUsersByUsernameAsync()
        {
            try
            {
                if (Username == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Must Enter a Valid Username", "Try Again", "OK");
                    return;
                }
                await _iRepository.GetByIdOrUsername(this, Username);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("VIEWMODEL EXCEPTION", ex.Message, "OK");
            }
        }

        private async Task DeleteUserAsync()
        {
            await _iRepository.DeleteAsync(this);
        }
        private async Task UpdateUserAsync()
        {
            await _iRepository.PutAsync(this);
        }


    }
}
