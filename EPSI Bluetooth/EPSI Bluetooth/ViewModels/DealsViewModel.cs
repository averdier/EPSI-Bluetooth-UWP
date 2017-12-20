using EPSI_Bluetooth.Helpers;
using EPSI_Bluetooth.Models;
using EPSI_Bluetooth.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EPSI_Bluetooth.ViewModels
{
    public class DealsViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private APIService _api;

        private VisualState _currentState;

        public bool IsViewState { get { return Selected != null && !IsLoading && _currentState.Name != NarrowStateName; } }

        private string _searchTitle;
        public string SearchTitle
        {
            get { return _searchTitle; }
            set
            {
                Set(ref _searchTitle, value);
                if (value != "" && value != string.Empty)
                {
                    UpdateSearch();
                }
            }
        }

        private string _selectedSearchStatus;
        public string SelectedSearchStatus
        {
            get { return _selectedSearchStatus; }
            set
            {
                Set(ref _selectedSearchStatus, value);
                if (value != "" && value != string.Empty)
                {
                    UpdateSearch();
                }
            }
        }


        private DealModel _selected;
        public DealModel Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        private string _loadingMessage;
        public string LoadingMessage
        {
            get { return _loadingMessage; }
            set { Set(ref _loadingMessage, value); }
        }

        private int _loadingColumnSpan;
        public int LoadingColumnSpan
        {
            get { return _loadingColumnSpan; }
            set { Set(ref _loadingColumnSpan, value); }
        }

        public ObservableCollection<DealModel> DealsItems { get; private set; } = new ObservableCollection<DealModel>();

        public DealsViewModel()
        {
            _isLoading = false;
            _selected = new DealModel();
            _api = new APIService(Helpers.Settings.Username, Helpers.Settings.Password);
        }

        public async void UpdateSearch()
        {
            await LoadDataAsync(_currentState);
        }

        public async Task LoadDataAsync(VisualState currentState)
        {
            LoadingColumnSpan = (currentState.Name == NarrowStateName) ? 1 : 2;
            IsLoading = true;
            OnPropertyChanged(nameof(IsViewState));
            _currentState = currentState;
            DealsItems.Clear();

            try
            {

                LoadingMessage = "Chargment des promotions";
                var data = await _api.GetDealContainerWithRetryAsync();
                foreach (var item in data.Deals)
                {
                    DealsItems.Add(item);
                }

                if (DealsItems.Count > 0)
                {
                    Selected = DealsItems[0];
                }

                IsLoading = false;
                OnPropertyChanged(nameof(IsViewState));
            }
            catch (Exception ex)
            {
                IsLoading = false;
                var errorDialog = new Windows.UI.Popups.MessageDialog(
                            ex.Message,
                            "Erreur");
                errorDialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });
                await errorDialog.ShowAsync();
            }
        }

        public void OnItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("OnItemClick");
            DealModel item = e?.ClickedItem as DealModel;
            if (item != null)
            {
                if (_currentState.Name == NarrowStateName)
                {
                    Views.ShellPage.ShellFrame.Navigate(typeof(Views.DealDetailPage), item);
                }
                else
                {
                    Debug.WriteLine("ici");
                    Selected = item;
                    OnPropertyChanged(nameof(IsViewState));
                }
            }
        }

        public async void OnRefreshItemsClick(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync(_currentState);
        }

        public async void OnDeleteItemClick(object sender, RoutedEventArgs e)
        {
            if (Selected != null)
            {
                var dialog = new Windows.UI.Popups.MessageDialog(
                    "Voulez vous supprimer la promotion ?",
                    "Attention"
                    );
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Oui") { Id = 0 });
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Non") { Id = 1 });

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                var result = await dialog.ShowAsync();

                if ((int)result.Id == 0)
                {
                    try
                    {
                        if (await _api.DeleteCustomerWithRetryAsync(Selected.Id))
                        {
                            DealsItems.Remove(Selected);
                            Selected = DealsItems.FirstOrDefault();
                            OnPropertyChanged(nameof(IsViewState));
                        }
                        else
                        {
                            var unknowErrorDialog = new Windows.UI.Popups.MessageDialog(
                                "Une erreur est survenue",
                                "Erreur");
                            unknowErrorDialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });
                            await unknowErrorDialog.ShowAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorDialog = new Windows.UI.Popups.MessageDialog(
                            ex.Message,
                            "Erreur");
                        errorDialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });
                        await errorDialog.ShowAsync();
                    }
                }
            }
        }

        public void OnEditItemClick(object sender, RoutedEventArgs e)
        {

        }

        public void OnAddItemClick(object sender, RoutedEventArgs e)
        {
            Views.ShellPage.ShellFrame.Navigate(typeof(Views.DealAddPage));
        }

        public void OnStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            _currentState = e.NewState;
            OnPropertyChanged(nameof(IsViewState));
            LoadingColumnSpan = (_currentState.Name == NarrowStateName) ? 1 : 2;
            Debug.WriteLine("StateChanged");
        }
    }
}
