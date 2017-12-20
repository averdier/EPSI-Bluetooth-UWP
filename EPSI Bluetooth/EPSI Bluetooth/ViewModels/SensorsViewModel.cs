using EPSI_Bluetooth.Helpers;
using EPSI_Bluetooth.Models;
using EPSI_Bluetooth.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPSI_Bluetooth.ViewModels
{
    public class SensorsViewModel : Observable
    {
        private APIService _api;

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

        public ObservableCollection<SensorModel> SensorsItems { get; private set; } = new ObservableCollection<SensorModel>();

        public SensorsViewModel()
        {
            _isLoading = false;;
            _api = new APIService(Helpers.Settings.Username, Helpers.Settings.Password);
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            SensorsItems.Clear();

            try
            {

                LoadingMessage = "Chargment des Sondes";
                var data = await _api.GetSensorContainerWithRetryAsync();
                foreach (var item in data.Sensors)
                {
                    SensorsItems.Add(item);
                }

                IsLoading = false;
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
    }
}
