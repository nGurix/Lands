using Lands.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lands.ViewModels
{
    public class MainViewModel
    {
        #region Properties
        public List<Land> LandsList { get; set; }
        #endregion
        #region ViewModels
        public LoginViewModel Login { get; set; }
        public LandsViewModel Lands { get; set; }
        public LandViewModel Land { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            Login = new LoginViewModel();
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
