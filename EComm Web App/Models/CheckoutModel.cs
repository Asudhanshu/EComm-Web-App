using System;
namespace EComm_Web_App.Models
{
    public class CheckoutModel
    {
        public int ShoppingCartId { get; set; }

        public string ProductName { get; set; }

        public int ProductId { get; set; }
        public int ProductPrice { get; set; }

        public int ProductQty { get; set; }
        public int TotalPrice { get; set; }
        public int FinalPrice { get; set; }


    }
}
