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
    public class DealAddViewModel : Observable
    {
        public bool SaveBtn_IsEnabled
        {
            get
            {
                return Label != null && Label != string.Empty 
                    && Description != null && Description != string.Empty
                    && StartAt != null && EndAt != null;
            }
        }

        private DateTime _startAt;
        public DateTime StartAt
        {
            get { return _startAt; }
            set
            {
                Set(ref _startAt, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private DateTime _endAt;
        public DateTime EndAt
        {
            get { return _endAt; }
            set
            {
                Set(ref _endAt, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private string _label;
        public string Label
        {
            get { return _label; }
            set
            {
                Set(ref _label, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                Set(ref _description, value);
                OnPropertyChanged(nameof(SaveBtn_IsEnabled));
            }
        }

        private APIService _api;

        public DealAddViewModel()
        {
            _api = new APIService(Helpers.Settings.Username, Helpers.Settings.Password);
            StartAt = DateTime.Now;
            EndAt = DateTime.Now;
        }

        public async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (SaveBtn_IsEnabled)
            {
                DealPostModel model = new DealPostModel
                {
                    start_at = StartAt.ToString("o"),
                    end_at = EndAt.ToString("o"),
                    label = Label,
                    description = Description
                };

                try
                {
                    var customer = await _api.PostDealWithRetryAsync(model);

                    if (customer != null)
                    {
                        Views.ShellPage.ShellFrame.Navigate(typeof(Views.DealsPage), customer);
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
