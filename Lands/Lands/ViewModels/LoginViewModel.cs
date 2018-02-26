using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Lands.ViewModels
{
    public class LoginViewModel
    {
        #region Properties
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsRunning { get; set; }
        public bool IsRemember { get; set; }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            IsRemember = true;
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }
        private void Login()
        {

        }
        #endregion
    }
}
