using EPSI_Bluetooth.Helpers;
using EPSI_Bluetooth.Models;
using EPSI_Bluetooth.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EPSI_Bluetooth.ViewModels
{
    public class DealSendViewModel : Observable
    {
        public bool SaveBtn_IsEnabled
        {
            get
            {
                return SelectedSend != null && SelectedSend != String.Empty && SelectedThresholds != null && SelectedThresholds != String.Empty;
            }
        }

        private DealModel _item;
        public DealModel Item
        {
            get { return _item; }
            set
            {
                Set(ref _item, value);
            }
        }

        public List<String> PossibleThresholds { get { return new List<string> { "1", "2", "3", "4", "5" }; } }

        private string _selectedThreshold;
        public string SelectedThresholds
        {
            get { return _selectedThreshold; }
            set { Set(ref _selectedThreshold, value); }
        }

        public List<String> PossibleSends { get { return new List<string> { "SMS", "EMAIL", "SMS & EMAIL" }; } }

        private string _selectedSend;
        public string SelectedSend
        {
            get { return _selectedSend; }
            set { Set(ref _selectedSend, value); }
        }
        private APIService _api;

        public DealSendViewModel(DealModel item)
        {
            Item = item;
            _api = new APIService(Helpers.Settings.Username, Helpers.Settings.Password);
        }

        

        public void ThresholdCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedThresh = e.AddedItems[0] as String;
            SelectedThresholds = selectedThresh;
            OnPropertyChanged(nameof(SaveBtn_IsEnabled));
        }

        public void SendCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSend = e.AddedItems[0] as String;
            SelectedSend = selectedSend;
            OnPropertyChanged(nameof(SaveBtn_IsEnabled));
        }

        public async void OnSendClick(object sender, RoutedEventArgs e)
        {
            if (SaveBtn_IsEnabled)
            {
                TaskPostModel model = new TaskPostModel
                {
                    deal_id = Item.Id,
                    time_threshold = Int32.Parse(SelectedThresholds),
                    send_options = new SendOptionsPostModel
                    {
                        email = false,
                        sms = false
                    }
                };


                if (SelectedSend == "SMS")
                {
                    model.send_options.sms = true;
                }

                if (SelectedSend == "EMAIL")
                {
                    model.send_options.email = true;
                }
                

                if (SelectedSend == "SMS & EMAIL")
                {
                    model.send_options.sms = true;
                    model.send_options.email = true;
                }

                try
                {
                    var task = await _api.PostSendTaskWithRetryAsync(model);

                    if (task != null)
                    {
                        Views.ShellPage.ShellFrame.Navigate(typeof(Views.DealsPage));
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
