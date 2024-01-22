namespace ECommRepo.Models
{
    /// <summary>
    /// Shopping Cart Model is used to map the data from Data Access Layer Shopping Cart Model
    /// </summary>
    public class ShoppingCartModel
    {
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public int ProductQty { get; set; }
        public string UserName { get; set; }
    }
}
