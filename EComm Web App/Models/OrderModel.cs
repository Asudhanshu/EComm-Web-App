using System;

namespace EComm_Web_App.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public int TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
