using Lands.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Lands.Services;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace Lands.ViewModels
{
    public class LandsViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<LandItemViewModel> lands; //se usa observableCollection cuando pintamos la lista en un listview
        private bool isRefreshing;
        private string filter;
        #endregion

        #region Properties
        public ObservableCollection<LandItemViewModel> Lands // de nuevo tiene que ser observablecollection por la listview
        {
            get { return lands; }
            set { SetValue(ref lands, value); }
        }
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetValue(ref isRefreshing, value); }
        }
        public string Filter
        {
            get { return filter; }
            set
            {
                SetValue(ref filter, value);
                Search();
            }
        }
        #endregion

        #region Constructors
        public LandsViewModel()
        {
            apiService = new ApiService();
            LoadLands();
        }
        #endregion

        #region Methods
        private async void LoadLands()
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Accept");
                await Application.Current.MainPage.Navigation.PopToRootAsync();
                return;
            }
            var apiLands = Application.Current.Resources["APILands"].ToString();
            var response = await apiService.GetList<Land>(
                apiLands,
                "/rest",
                "/v2/all");
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }
            MainViewModel.GetInstance().LandsList = (List<Land>)response.Result;
            Lands = new ObservableCollection<LandItemViewModel>(ToLandItemViewModel());
            IsRefreshing = false;
        }

        private IEnumerable<LandItemViewModel> ToLandItemViewModel()
        {
            return MainViewModel.GetInstance().LandsList.Select(l => new LandItemViewModel
            {
                Alpha2Code = l.Alpha2Code,
                Alpha3Code = l.Alpha3Code,
                AltSpellings = l.AltSpellings,
                Area = l.Area,
                Borders = l.Borders,
                CallingCodes = l.CallingCodes,
                Capital = l.Capital,
                Cioc = l.Cioc,
                Currencies = l.Currencies,
                Demonym = l.Demonym,
                Flag = l.Flag,
                Gini = l.Gini,
                Languages = l.Languages,
                Latlng = l.Latlng,
                Name = l.Name,
                NativeName = l.NativeName,
                NumericCode = l.NumericCode,
                Population = l.Population,
                Region = l.Region,
                RegionalBlocs = l.RegionalBlocs,
                Subregion = l.Subregion,
                Timezones = l.Timezones,
                TopLevelDomain = l.TopLevelDomain,
                Translations = l.Translations,
            });
        }
        #endregion
        #region Command
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadLands);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        private void Search()
        {
            if(string.IsNullOrEmpty(Filter))
            {
                Lands = new ObservableCollection<LandItemViewModel>(ToLandItemViewModel());
            }
            else
            {
                Lands = new ObservableCollection<LandItemViewModel>(
                    ToLandItemViewModel().Where(l => l.Name.ToLower().Contains(Filter.ToLower()) ||
                                        l.Capital.ToLower().Contains(Filter.ToLower())));
            }
        }
        #endregion
    }
}
