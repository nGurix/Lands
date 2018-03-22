using GalaSoft.MvvmLight.Command;
using Lands.Helpers;
using Lands.Models;
using Lands.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    public class MyProfileViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion  

        #region Properties
        public UserLocal User { get; set; }
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set { SetValue(ref imageSource, value); }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetValue(ref isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set { SetValue(ref isRunning, value); }
        }       
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource; 
        private MediaFile file;
        #endregion

        #region Constructors
        public MyProfileViewModel()
        {
            apiService = new ApiService();
            User = MainViewModel.GetInstance().User;
            ImageSource = User.ImageFullPath;
            IsEnabled = true;
        }
        #endregion

        #region Command
        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                var source = await Application.Current.MainPage.DisplayActionSheet(
                    Languages.SourceImageQuestion,
                    Languages.Cancel,
                    null,
                    Languages.FromGallery,
                    Languages.FromCamera);

                if (source == Languages.Cancel)
                {
                    file = null;
                    return;
                }

                if (source == Languages.FromCamera)
                {
                    file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(User.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.FirstNameValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LastNameValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(User.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }

            if (!RegexUtilities.IsValidEmail(User.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation2,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(User.Telephone))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PhoneValidation,
                    Languages.Accept);
                return;
            }          

            IsRunning = true;
            IsEnabled = false;

            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    checkConnetion.Message,
                    Languages.Accept);
                return;
            }

            byte[] imageArray = null;
            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
            }

            var userDomain = Converter.ToUserDomain(User, imageArray);
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await apiService.Put(apiSecurity, "/api", "/Users", MainViewModel.GetInstance().TokenType,MainViewModel.GetInstance().Token, userDomain);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            IsRunning = false;
            IsEnabled = true;

            await Application.Current.MainPage.DisplayAlert(
                Languages.ConfirmLabel,
                Languages.UserRegisteredMessage,
                Languages.Accept);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        #endregion
    }
}
