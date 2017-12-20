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
    public class CustomerAddViewModel : Observable
    {
        public bool SaveBtn_IsEnabled
        {
            get
            {
                return FirstName != null && FirstName != string.Empty && LastName != null && LastName != string.Empty;
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set {
                Set(ref _lastName, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set {
                Set(ref _firstName, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { Set(ref _phoneNumber, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        private string _bluetoothMacAddress;
        public string BluetoothMacAddress
        {
            get { return _bluetoothMacAddress; }
            set { Set(ref _bluetoothMacAddress, value); }
        }

        private APIService _api;
        public CustomerAddViewModel()
        {
            _api = new APIService(Helpers.Settings.Username, Helpers.Settings.Password);
        }



        public async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (SaveBtn_IsEnabled)
            {
                CustomerPostModel model = new CustomerPostModel
                {
                    last_name = LastName,
                    first_name = FirstName,
                    email = Email,
                    phone_number = PhoneNumber,
                    bluetooth_mac_address = BluetoothMacAddress
                };

                try
                {
                    var customer = await _api.PostCustomerWithRetryAsync(model);

                    if (customer != null)
                    {
                        Views.ShellPage.ShellFrame.Navigate(typeof(Views.CustomersPage), customer);
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
