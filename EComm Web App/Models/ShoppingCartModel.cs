using System;
namespace EComm_Web_App.Models
{
    public class ShoppingCartModel
    {
        public int ShoppingCartId { get; set; }

        public string ProductName { get; set; }

        public int ProductId { get; set; }
        public int ProductPrice { get; set; }

        public int ProductQty { get; set; }

        public string UserName { get; set; }
    }
}
