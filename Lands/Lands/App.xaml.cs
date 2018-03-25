
using Xamarin.Forms;
using Lands.Views;
using Lands.Helpers;
using Lands.ViewModels;
using Lands.Services;
using Lands.Models;
using System;

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
