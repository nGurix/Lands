using Lands.Domain;
using Lands.Helpers;
using Lands.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lands.ViewModels
{
    public class MainViewModel
    {
        #region Properties
        public List<Land> LandsList { get; set; }
        public string Token { get; set; }
        public string TokenType { get; set; }
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }
        public User User { get; set; }
        #endregion

        #region ViewModels
        public RegisterViewModel Register { get; set; }
        public LoginViewModel Login { get; set; }
        public LandsViewModel Lands { get; set; }
        public LandViewModel Land { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            Login = new LoginViewModel();
            LoadMenu();
        }
        #endregion

        #region Methods
        private void LoadMenu()
        {
            Menus = new ObservableCollection<MenuItemViewModel>();
            Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_settings",
                Title = Languages.MyProfile,
                PageName = "MyProfilePage"
            });
            Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_insert_chart",
                Title = Languages.Statics,
                PageName = "StaticsPage"
            });
            Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                Title = Languages.LogOut,
                PageName = "LoginPage"
            });
        }
        #endregion

        #region Singleton
        //permite hacer un llamado de la mainviewmodel desde cualquier clase sin necesidad de instancear otra MainViewModel 
        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if(instance == null)
            {
                return new MainViewModel(); 
            }
            return instance;
        }
        #endregion
    }
}
