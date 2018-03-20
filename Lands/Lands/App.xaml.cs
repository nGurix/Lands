
using Xamarin.Forms;
using Lands.Views;
using Lands.Helpers;
using Lands.ViewModels;
using Lands.Services;
using Lands.Models;

namespace Lands
{
    public partial class App : Application
    {
        #region Properties
        public static NavigationPage Navigation { get; internal set; }
        #endregion

        #region Constructors
        public App()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(Settings.Token))
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                DataService dataService = new DataService();
                var user = dataService.First<UserLocal>(false);

                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = Settings.Token;
                mainViewModel.Lands = new LandsViewModel();
                mainViewModel.TokenType = Settings.TokenType;
                mainViewModel.User = user;

                MainPage = new MasterPage();
            }

        }
        #endregion

        #region Methods
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
