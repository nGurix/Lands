using GalaSoft.MvvmLight.Command;
using Lands.Helpers;
using Lands.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    public class MenuItemViewModel
    {
        #region Properties
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
        }

        private void Navigate()
        {
            App.Master.IsPresented = false;//para ocultar el menu hamburguesa 

            if (PageName == "LoginPage")
            {
                //los token se borran al deslogearse
                Settings.IsRemember = "false";
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = null;
                mainViewModel.User = null;

                //Application.Current.MainPage = new LoginPage();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else if (PageName == "MyProfilePage")
            {
                MainViewModel.GetInstance().MyProfile = new MyProfileViewModel();
                App.Navigation.PushAsync(new MyProfilePage());
            }
        }
        #endregion
    }
}
