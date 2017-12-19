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
    public class CustomersViewModel : Observable
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


        private CustomerModel _selected;
        public CustomerModel Selected
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

        public ObservableCollection<CustomerModel> CustomersItems { get; private set; } = new ObservableCollection<CustomerModel>();

        public CustomersViewModel()
        {
            _isLoading = false;
            _selected = new CustomerModel();
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
            CustomersItems.Clear();

            try
            {
                
                LoadingMessage = "Chargment des clients";
                var data = await _api.GetCustomerContainerWithRetryAsync();
                foreach (var item in data.Customers)
                {
                    CustomersItems.Add(item);
                }

                if (CustomersItems.Count > 0)
                {
                    Selected = CustomersItems[0];
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
            CustomerModel item = e?.ClickedItem as CustomerModel;
            if (item != null)
            {
                if (_currentState.Name == NarrowStateName)
                {
                    //NavigationService.Navigate<Views.NeedDetailPage>(item);
                }
                else
                {
                    Debug.WriteLine("ici");
                    Selected = item;
                    OnPropertyChanged(nameof(IsViewState));
                }
            }
        }
    }
}
