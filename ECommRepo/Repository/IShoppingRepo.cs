using ECommRepo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ECommRepo.Repository
{
    /// <summary>
    /// IShoppingRepo Interface is the Interface in Business Logic Layer to fetch the data from DataAccess Layer and pass it to ECommAPI
    /// </summary>
    public interface IShoppingRepo
    {
        Task<ShoppingCartModel> AddToCart(ShoppingCartModel shoppingCartModel);
        Task<List<ShoppingCartModel>> GetCart(string name);
        Task DeleteCart(int id);
        Task<ShoppingCartModel> GetCartById(int id);
        Task<ShoppingCartModel> UpdateCart(ShoppingCartModel cartModel);
        Task<OrderModel> OrderItem(OrderModel orderModel);
        Task<ProductModel> UpdateProductQuantity(int Id, int Qty);
        Task<List<OrderModel>> GetOrders(string name);
        List<ShoppingCartModel> GetShoppingByList();
        void UpdateShoppingByList(ShoppingCartModel shoppingCartModel);
        ShoppingCartModel CreateTest(ShoppingCartModel shoppingCartModel);
    }
}
