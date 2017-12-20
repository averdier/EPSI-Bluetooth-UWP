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
        public string Email { get; set; }
        public string Bluetooth_Mac_Address { get; set; }
        public string Phone_Number { get; set; }
    }

    public class CustomerPostModel
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string bluetooth_mac_address { get; set; }
        public string phone_number { get; set; }
    }

    public class CustomerContainer
    {
        public List<CustomerModel> Customers { get; set; }
    }


}
