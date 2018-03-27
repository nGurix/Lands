
using Xamarin.Forms;
using Lands.Views;
using Lands.Helpers;
using Lands.ViewModels;
using Lands.Services;
using Lands.Models;
using System;
using System.Threading.Tasks;

namespace Lands
{
    public partial class App : Application
    {
        #region Properties
        public static NavigationPage Navigation { get; internal set; }
        public static MasterPage Master { get; internal set; }
        #endregion

        #region Constructors
        public App()
        {
            InitializeComponent();

            if (Settings.IsRemember == "true")
            {
                DataService dataService = new DataService();
                var token = dataService.First<TokenResponse>(false);

                if(token != null && token.Expires > DateTime.Now)
                {

                    var user = dataService.First<UserLocal>(false);
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Token = token;
                    mainViewModel.User = user;

                    mainViewModel.Lands = new LandsViewModel();
                    MainPage = new MasterPage();
                }
                else
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }

        }
        #endregion

        #region Methods
        public static Action HideLoginView
        {
            get
            {
                return new Action(() => Current.MainPage =
                                  new NavigationPage(new LoginPage()));
            }
        }

        public static async Task NavigateToProfile(FacebookResponse profile)
        {
            if (profile == null)
            {
                Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            var apiService = new ApiService();
            var dataService = new DataService();

            var apiSecurity = Current.Resources["APISecurity"].ToString();
            var token = await apiService.LoginFacebook(
                apiSecurity,
                "/api",
                "/Users/LoginFacebook",
                profile);

            if (token == null)
            {
                Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            var user = await apiService.GetUserByEmail(
                apiSecurity,
                "/api",
                "/Users/GetUserByEmail",
                token.TokenType,
                token.AccessToken,
                token.UserName);

            UserLocal userLocal = null;
            if (user != null)
            {
                userLocal = Converter.ToUserLocal(user);
                dataService.DeleteAllAndInsert(userLocal);
                dataService.DeleteAllAndInsert(token);
            }

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;
            mainViewModel.User = userLocal;
            //mainViewModel.Lands = new LandsViewModel();
            //Current.MainPage = new MasterPage();
            Settings.IsRemember = "true";

            mainViewModel.Lands = new LandsViewModel();
            Current.MainPage = new MasterPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        #endregion
    }
}
