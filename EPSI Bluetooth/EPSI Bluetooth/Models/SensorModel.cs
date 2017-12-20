using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPSI_Bluetooth.Models
{
    public class SensorModel
    {
        public string Id { get; set; }
        public int Pos_y { get; set; }
        public int Pos_x { get; set; }
        public int Radius { get; set; }

        public string ImagePath { get; private set; } = "ms-appx:///Assets/bluetooth.png";
    }

    public class SensorContainer
    {
        public List<SensorModel> Sensors { get; set; }
    }
}
