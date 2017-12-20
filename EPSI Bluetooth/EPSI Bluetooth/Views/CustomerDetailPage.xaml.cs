using EPSI_Bluetooth.Models;
using EPSI_Bluetooth.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace EPSI_Bluetooth.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class CustomerDetailPage : Page
    {
        public CustomerDetailViewModel ViewModel { get; } = new CustomerDetailViewModel();

        public CustomerDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.LoadData(e.Parameter as CustomerModel);

            this.Loaded += CustomerDetailPage_Loaded;
        }

        private void CustomerDetailPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (WindowStates.CurrentState.Name == "WideState")
            {
                if (Views.ShellPage.ShellFrame.CanGoBack)
                {
                    Views.ShellPage.ShellFrame.GoBack();
                }
            }
        }
    }
}
