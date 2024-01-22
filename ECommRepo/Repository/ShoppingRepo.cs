using ECommRepo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using EComm_Web_App.Models;
using AutoMapper;

namespace ECommRepo.Repository
{
    /// <summary>
    /// ShoppingRepo Class is the class in Business Logic Layer to fetch the data from DataAccess Layer and pass it to ECommAPI
    /// </summary>
    public class ShoppingRepo : IShoppingRepo
    {
        private readonly ECommDbContext _context;
        private readonly IMapper _mapper;
        /// <summary>
        /// Constructor to initialize the property
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public ShoppingRepo(ECommDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// This Method is used to decrease the Quantity of Products available in the database on Clicking on Checkout
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ProductModel> UpdateProductQuantity(int Id, int Qty)
        {
            var products = await _context.product.FindAsync(Id);
            products.ProductQty = products.ProductQty - Qty;
            try
            {
                _context.product.Update(products);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            await _context.SaveChangesAsync();
            ProductModel model = new ProductModel();
            return model;
        }
        /// <summary>
        /// Add to Cart Method is used to add the items in User Cart
        /// </summary>
        /// <param name="shoppingCartModel"></param>
        /// <returns></returns>
        public async Task<ShoppingCartModel> AddToCart(ShoppingCartModel shoppingCartModel)
        {
            var list = await _context.ShoppingCart.Select(x => new ShoppingCartModel
            {
                ShoppingCartId = x.ShoppingCartId,
                ProductId = x.ProductId,
                UserName = x.UserName
            }).ToListAsync();
            int flag = 0;
            foreach (var item in list)
            {
                if (item.ProductId == shoppingCartModel.ProductId && item.UserName.Equals(shoppingCartModel.UserName))
                {
                    flag = item.ShoppingCartId;
                }
            }
            if (flag == 0)
            {
                ShoppingCart shoppingCart = new ShoppingCart();
                _mapper.Map(shoppingCartModel, shoppingCart);
                _context.ShoppingCart.Add(shoppingCart);
                await _context.SaveChangesAsync();
            }
            else if (flag != 0)
            {
                ShoppingCart shoppingCart = await _context.ShoppingCart.FindAsync(flag);
                PropertyCopy<ShoppingCartModel, ShoppingCart>.Copy(shoppingCartModel, shoppingCart);
                shoppingCart.ProductQty = shoppingCart.ProductQty + 1;
                _context.ShoppingCart.Update(shoppingCart);
                await _context.SaveChangesAsync();
            }
            return shoppingCartModel;
        }
        /// <summary>
        /// Get the available values in the Cart based on the user name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<ShoppingCartModel>> GetCart(string name)
        {
            var list = await _context.ShoppingCart.Select(x => new ShoppingCartModel
            {
                ShoppingCartId = x.ShoppingCartId,
                ProductName = x.ProductName,
                UserName = x.UserName,
                ProductPrice = x.ProductPrice,
                ProductQty = x.ProductQty,
                ProductId = x.ProductId
            }).ToListAsync();
            return list.Where(x => x.UserName == name).ToList();
        }
        /// <summary>
        /// To display the orders based on the user in View Order page
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<OrderModel>> GetOrders(string name)
        {
            var list = await _context.order.Select(x => new OrderModel
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                UserName = x.UserName,
                TotalPrice = x.TotalPrice
            }).ToListAsync();
            return list.Where(x => x.UserName == name).ToList();
        }
        /// <summary>
        /// Delete all the items from the Cart
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteCart(int id)
        {
            var cart = await _context.ShoppingCart.FindAsync(id);
            if (cart == null)
            {
                return;
            }
            _context.ShoppingCart.Remove(cart);
            await _context.SaveChangesAsync();
            return;
        }
        /// <summary>
        /// Get the Cart for Cart Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ShoppingCartModel> GetCartById(int id)
        {
            var cart1 = await _context.ShoppingCart.FindAsync(id);
            ShoppingCartModel cart = new ShoppingCartModel();
            _mapper.Map(cart1, cart);
            return cart;
        }
        /// <summary>
        /// To Reduce the product Quantity by 1 in the cart on Removing from Cart
        /// </summary>
        /// <param name="cartModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ShoppingCartModel> UpdateCart(ShoppingCartModel cartModel)
        {
            int id = cartModel.ShoppingCartId;
            var carts = await _context.ShoppingCart.FindAsync(id);
            carts.ProductQty = cartModel.ProductQty - 1;
            try
            {
                _context.ShoppingCart.Update(carts);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            await _context.SaveChangesAsync();
            return cartModel;
        }
        /// <summary>
        /// To order the Item present in the cart
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        public async Task<OrderModel> OrderItem(OrderModel orderModel)
        {
            Order orders = new Order();
            PropertyCopy<OrderModel, Order>.Copy(orderModel, orders);
            orders.OrderDate = DateTime.Now;
            _context.order.Add(orders);
            await _context.SaveChangesAsync();
            return orderModel;
        }
        /// <summary>
        /// Functions for Unit Testing
        /// </summary>
        /// <returns></returns>
        public List<ShoppingCartModel> GetShoppingByList()
        {
            return _context.ShoppingCart.Select(shopping =>

                new ShoppingCartModel
                {
                    ShoppingCartId = shopping.ShoppingCartId,
                    ProductName = shopping.ProductName,
                    ProductId = shopping.ProductId,
                    ProductPrice = shopping.ProductPrice,
                    ProductQty = shopping.ProductQty,
                    UserName = shopping.UserName
                }
            ).ToList();
        }
        public void UpdateShoppingByList(ShoppingCartModel shoppingCartModel)
        {
            try
            {
                var shoppingData = _context.ShoppingCart.Find(shoppingCartModel.ShoppingCartId);
                PropertyCopy<ShoppingCartModel, ShoppingCart>.Copy(shoppingCartModel, shoppingData);
                //_mapper.Map(shoppingCartModel, shoppingData);
                _context.ShoppingCart.Update(shoppingData);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public ShoppingCartModel CreateTest(ShoppingCartModel shoppingCartModel)
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            PropertyCopy<ShoppingCartModel, ShoppingCart>.Copy(shoppingCartModel, shoppingCart);
            _context.ShoppingCart.Add(shoppingCart);
            _context.SaveChanges();
            return shoppingCartModel;
        }
    }
}
