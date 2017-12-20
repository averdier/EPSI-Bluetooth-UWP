using EPSI_Bluetooth.Helpers;
using EPSI_Bluetooth.Models;
using EPSI_Bluetooth.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace EPSI_Bluetooth.ViewModels
{
    public class CustomerDetailViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private APIService _api;

        private CustomerModel _item;
        public CustomerModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
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

        public CustomerDetailViewModel()
        {
            _api = new APIService(Helpers.Settings.Username, Helpers.Settings.Password);
        }

        public void LoadData(CustomerModel item)
        {
            Item = item;
        }

        public async void OnRefreshItemClick(object sender, RoutedEventArgs e)
        {

        }

        public async void OnDeleteItemClick(object sender, RoutedEventArgs e)
        {

        }

        public void OnStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            Debug.WriteLine("State changed");
            if (e.OldState.Name == NarrowStateName && e.NewState.Name == WideStateName)
            {
                if (Views.ShellPage.ShellFrame.CanGoBack)
                {
                    Views.ShellPage.ShellFrame.GoBack();
                }
            }
        }
    }
}
