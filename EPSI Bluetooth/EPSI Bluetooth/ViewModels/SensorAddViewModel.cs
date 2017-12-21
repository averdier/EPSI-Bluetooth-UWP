using EPSI_Bluetooth.Helpers;
using EPSI_Bluetooth.Models;
using EPSI_Bluetooth.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace EPSI_Bluetooth.ViewModels
{
    public class SensorAddViewModel : Observable
    {
        public bool SaveBtn_IsEnabled
        {
            get
            {
                return PosX != null && PosX != String.Empty && PosY != null && PosY != String.Empty && Radius != null 
                    && Radius != String.Empty && Username != null && Username != String.Empty && Password != null && Password != String.Empty 
                    && Server != null && Server != String.Empty && Key != null && Key != String.Empty && Port != null && Port != String.Empty && KeepAlive != null && KeepAlive != String.Empty;
            }
        }

        private string _posX;
        public string PosX
        {
            get { return _posX; }
            set
            {
                Set(ref _posX, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private string _posY;
        public string PosY
        {
            get { return _posY; }
            set
            {
                Set(ref _posY, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private string _radius;
        public string Radius
        {
            get { return _radius; }
            set
            {
                Set(ref _radius, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                Set(ref _username, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                Set(ref _password, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private string _server;
        public string Server
        {
            get { return _server; }
            set
            {
                Set(ref _server, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private string _port;
        public string Port
        {
            get { return _port; }
            set
            {
                Set(ref _port, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private string _keepAlive;
        public string KeepAlive
        {
            get { return _keepAlive; }
            set
            {
                Set(ref _keepAlive, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set
            {
                Set(ref _key, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private APIService _api;

        public SensorAddViewModel()
        {
            _api = new APIService(Helpers.Settings.Username, Helpers.Settings.Password);
        }

        public async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (SaveBtn_IsEnabled)
            {
                SensorPostModel model = new SensorPostModel
                {
                    pos_x = Int32.Parse(PosX),
                    pos_y = Int32.Parse(PosY),
                    radius = Int32.Parse(Radius),
                    key = Key,
                    mqtt_account = new MqttAccountPostModel
                    {
                        server = Server,
                        port = Int32.Parse(Port),
                        keep_alive = Int32.Parse(KeepAlive),
                        username = Username,
                        password = Password
                    }
                };

                try
                {
                    var customer = await _api.PostSensorWithRetryAsync(model);

                    if (customer != null)
                    {
                        Views.ShellPage.ShellFrame.Navigate(typeof(Views.SensorsPage), customer);
                    }
                    else
                    {
                        var unknowErrordialog = new Windows.UI.Popups.MessageDialog(
                            "Une erreur est survenue",
                            "Erreur");
                        unknowErrordialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                        unknowErrordialog.DefaultCommandIndex = 0;

                        var resultUnknow = await unknowErrordialog.ShowAsync();
                    }
                }
                catch (Exception ex)
                {
                    var dialog = new Windows.UI.Popups.MessageDialog(
                    ex.Message,
                    "Erreur"
                    );
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });

                    dialog.DefaultCommandIndex = 0;

                    var result = await dialog.ShowAsync();
                }
            }
        }

        public void OnCancelClick(object sender, RoutedEventArgs e)
        {
            if (Views.ShellPage.ShellFrame.CanGoBack)
            {
                Views.ShellPage.ShellFrame.GoBack();
            }
        }
    }
}
