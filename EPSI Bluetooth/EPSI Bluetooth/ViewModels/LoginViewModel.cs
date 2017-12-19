using EPSI_Bluetooth.Helpers;
using EPSI_Bluetooth.Services;
using EPSI_Bluetooth.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EPSI_Bluetooth.ViewModels
{
    public class LoginViewModel : Observable
    {
        private APIService _api;

        private string _username;
        public string Username
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
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

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { Set(ref _errorMessage, value); }
        }

        public LoginViewModel()
        {
            IsLoading = false;
            ErrorMessage = "";
            _api = new APIService();
        }

        public async void OnLoginClick(object sender, RoutedEventArgs e)
        {
            Task<Boolean> loginTask = _api.LoginAsync(_username, _password);

            IsLoading = true;
            LoadingMessage = "Login, please wait ...";

            try
            {
                bool success = await loginTask;

                if (success)
                {
                    await Task.Run(() =>
                    {
                        Helpers.Settings.Username = _username;
                        Helpers.Settings.Password = _password;
                    });

                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(ShellPage));
                }
                else
                {
                    IsLoading = false;
                    ErrorMessage = "Error, please try later";
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Debug.WriteLine("X:LoginViewModel exception" + ex.Message);
                ErrorMessage = ex.Message;

            }
        }
    }
}
