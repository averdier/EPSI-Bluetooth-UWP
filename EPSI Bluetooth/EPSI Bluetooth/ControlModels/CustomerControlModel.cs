using EPSI_Bluetooth.Helpers;
using EPSI_Bluetooth.Models;
using EPSI_Bluetooth.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPSI_Bluetooth.ControlModels
{
    public class CustomerControlModel : Observable
    {
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

        private APIService _api;

        public CustomerControlModel()
        {
            IsLoading = false;
            _api = new APIService(Helpers.Settings.Username, Helpers.Settings.Password);
        }

        public async void OnMasterItemChanged(CustomerModel item)
        {
            IsLoading = true;
            LoadingMessage = "Chargement du client";

            try
            {
                Item = await _api.GetCustomerFromIdWithRetryAsync(item.Id);
            }

            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task canceled");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            IsLoading = false;
            LoadingMessage = "";
        }
    }
}
