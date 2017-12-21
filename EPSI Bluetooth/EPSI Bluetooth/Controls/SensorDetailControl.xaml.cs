using EPSI_Bluetooth.ControlModels;
using EPSI_Bluetooth.Models;
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

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page https://go.microsoft.com/fwlink/?LinkId=234236

namespace EPSI_Bluetooth.Controls
{
    public sealed partial class SensorDetailControl : UserControl
    {
        public SensorModel MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SensorModel; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SensorModel), typeof(SensorDetailControl), new PropertyMetadata(null));
        public SensorControlModel ControlModel { get; private set; }
        public SensorDetailControl()
        {
            this.InitializeComponent();
            ControlModel = new SensorControlModel();
            this.RegisterPropertyChangedCallback(MasterMenuItemProperty, OnMasterMenuItemPropertyChanged);
        }

        private void OnMasterMenuItemPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (dp == MasterMenuItemProperty)
            {
                ControlModel.OnMasterItemChanged(MasterMenuItem);
            }
            else
            {
                ControlModel.Item = null;
            }
        }
    }
}
