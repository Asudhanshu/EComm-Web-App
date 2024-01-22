using System;
namespace ECommRepo.Models
{
    /// <summary>
    /// Order Model is used to map the data from Data Access Layer Order Model
    /// </summary>
    public class OrderModel
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public int TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
