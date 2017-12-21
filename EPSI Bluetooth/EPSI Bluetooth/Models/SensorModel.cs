using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPSI_Bluetooth.Models
{
    public class MqttAccountModel
    {
        public string Device_Topic { get; set; }
        public int Keep_Alive { get; set; }
        public string StrKeepAlive { get { return Keep_Alive.ToString(); } }
        public string Clients_Topic { get; set; }
        public string Username { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string StrPort { get { return Port.ToString(); } }
    }

    public class MqttAccountPostModel
    {
        public string server { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int port { get; set; }
        public int keep_alive { get; set; }
    }

    public class SensorPostModel
    {
        public int pos_x { get; set; }
        public int pos_y { get; set; }
        public int radius { get; set; }
        public string key { get; set; }
        public MqttAccountPostModel mqtt_account { get; set; }
    }

    public class SensorModel
    {
        public string Id { get; set; }
        public int Pos_y { get; set; }
        public int Pos_x { get; set; }
        public int Radius { get; set; }

        public string StrPosX { get { return Pos_x.ToString(); } }
        public string StrPosY { get { return Pos_y.ToString(); } }
        public string StrRadius { get { return Radius.ToString();  } }

        public string ImagePath { get; private set; } = "ms-appx:///Assets/bluetooth.png";

        public MqttAccountModel Mqtt_Account { get; set; }
    }

    public class SensorContainer
    {
        public List<SensorModel> Sensors { get; set; }
    }
}
