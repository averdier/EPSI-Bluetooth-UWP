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
    }

    public class DealContainer
    {
        public List<DealModel> Deals { get; set; }
    }
}
