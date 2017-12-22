using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPSI_Bluetooth.Models
{
    public class TaskModel
    {
        public string Task_Id { get; set; }
    }


    public class SendOptionsPostModel
    {
        public bool email { get; set; }
        public bool sms { get; set; }
    }

    public class TaskPostModel
    {
        public int time_threshold { get; set; }
        public string deal_id { get; set; }
        public SendOptionsPostModel send_options { get; set; }
    }
}
