using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPSI_Bluetooth.Models
{
    public class DealModel
    {
        public string Id { get; set; }
        public string Start_At { get; set; }
        public string End_At { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
    }

    public class DealContainer
    {
        public List<DealModel> Deals { get; set; }
    }

    public class DealPostModel
    {
        public string start_at { get; set; }
        public string end_at { get; set; }
        public string label { get; set; }
        public string description { get; set; }
    }
}
