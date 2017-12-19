using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPSI_Bluetooth.Models
{
    public class CustomerModel
    {
        public string Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string FullName { get { return First_Name + " " + Last_Name; } }
        public string Postal_Code { get; set; }
    }

    public class CustomerContainer
    {
        public List<CustomerModel> Customers { get; set; }
    }
}
