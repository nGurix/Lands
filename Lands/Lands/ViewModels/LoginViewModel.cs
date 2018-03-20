using GalaSoft.MvvmLight.Command;
using Lands.Helpers;
using Lands.Services;
using Lands.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Attributes
        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties

        public string Email
        {
            get { return email; }
            set { SetValue(ref email, value); }
        }
        public string Password
        {
            get { return password; }
            set { SetValue(ref password, value); }
        }
        public bool IsRunning
        {
            get { return isRunning; }
            set { SetValue(ref isRunning, value); }
        }
        public bool IsRemember { get; set; }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetValue(ref isEnabled, value); }
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            apiService = new ApiService();
            dataService = new DataService();

            IsRemember = true;
            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand RegisterCommand
        {
            get
            {
                return new RelayCommand(Register);
            }
        }

        private async void Register()
        {
            MainViewModel.GetInstance().Register = new RegisterViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }
        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.EmailValidation, Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordValidation, Languages.Accept);
                return;
            }
            
            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if(!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                Password = string.Empty;
                return;
            }

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var token = await apiService.GetToken(apiSecurity, Email, Password);
            if(token == null)
            {
                IsRunning = false;
                IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.SomethingWrong, Languages.Accept);
                Password = string.Empty;
                return;
            }

            if(string.IsNullOrEmpty(token.AccessToken))
            {
                IsRunning = false;
                IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, token.ErrorDescription, Languages.Accept);
                Password = string.Empty;
                return;
            }

            var user = await apiService.GetUserByEmail(apiSecurity, "/api", "/Users/GetUserByEmail", token.TokenType, token.AccessToken, Email);

            var userLocal = Converter.ToUserLocal(user);

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token.AccessToken;
            mainViewModel.TokenType = token.TokenType;
            mainViewModel.User = userLocal;            

            if (IsRemember)
            {
                //el token se guarda en persistencia
                Settings.Token = token.AccessToken;
                Settings.TokenType = token.TokenType;
                dataService.DeleteAllAndInsert(userLocal);
            }
            //se referecia el singleton, asi se asegura que la landviewmodel queda alinieada a la LAndsPage
            mainViewModel.Lands = new LandsViewModel();

            Application.Current.MainPage = new MasterPage(); 

            IsRunning = false;
            IsEnabled = true;

            Email = string.Empty;
            Password = string.Empty;

        }
        #endregion
    }
}
