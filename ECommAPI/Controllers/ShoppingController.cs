using ECommRepo.Models;
using ECommRepo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ECommAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class ShoppingController : ControllerBase
    {
        //ReadOnly Property
        private readonly IShoppingRepo _repo;
        private readonly ILogger<ShoppingController> _logger;
        /// <summary>
        /// Constructor to initialize the IProductRepo
        /// </summary>
        /// <param name="repo"></param>
        ///<param name="logger"></param>
        public ShoppingController(IShoppingRepo repo, ILogger<ShoppingController> logger)
        {
            _logger = logger;
            _repo = repo;
        }
        // GET: api/Products
        /// <summary>
        /// To get the list of Available products in the cart of user from the Repository
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCart/{name}")]
        public async Task<ActionResult<IEnumerable<ShoppingCartModel>>> GetCart(string name)
        {
            try
            {
                return await _repo.GetCart(name);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        /// <summary>
        /// To get the order detail of the user from database
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("GetOrder/{name}")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrders(string name)
        {
            try
            {
                return await _repo.GetOrders(name);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        // GET: api/Products/5
        /// <summary>
        /// This function is used to get the cart for the user by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingCartModel>> GetCartById(int id)
        {
            try
            {
                _logger.LogInformation("Entering in ProductAPI ProductController GetProduct by Id method");
                var cart = await _repo.GetCartById(id);
                if (cart == null)
                {
                    _logger.LogInformation("Exiting from ProductAPI ProductController GetProduct by Id because no Product found");
                    return NotFound();
                }
                _logger.LogInformation("Exiting from ProductAPI ProductController GetProduct by Id method Successfully");
                return cart;
            }
            catch (Exception e)
            {
                _logger.LogError("Got the error in ProductAPI ProductController while fetching the data from repository" + e.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// This function is used to add the items into the cart
        /// </summary>
        /// <param name="shoppingCartModel"></param>
        /// <returns></returns>
        [HttpPost("ShoppingCart")]
        public async Task<ActionResult<ShoppingCartModel>> PostCart(ShoppingCartModel shoppingCartModel)
        {
            try
            {
                _logger.LogInformation("Entering in ProductAPI ProductController PostProduct method");
                await _repo.AddToCart(shoppingCartModel);
                _logger.LogInformation("Exiting from ProductAPI ProductController PostProduct method");

                //to be done
                return CreatedAtAction("GetProduct", new { id = shoppingCartModel.ShoppingCartId }, shoppingCartModel);
            }
            catch (Exception e)
            {
                _logger.LogError("Got the error in ProductAPI ProductController while creating the data into repository" + e.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// This function is used to order the items present in the cart
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        [HttpPost("order")]
        public async Task<ActionResult<OrderModel>> PostOrder(OrderModel orderModel)
        {
            try
            {
                _logger.LogInformation("Entering in ProductAPI ProductController PostProduct method");
                await _repo.OrderItem(orderModel);
                _logger.LogInformation("Exiting from ProductAPI ProductController PostProduct method");

                //to be done
                return CreatedAtAction("GetOrders", new { Name = orderModel.UserName }, orderModel);
            }
            catch (Exception e)
            {
                _logger.LogError("Got the error in ProductAPI ProductController while creating the data into repository" + e.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// This function is used to update the cart based on the Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cartModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ShoppingCartModel>> PutCart(int id, ShoppingCartModel cartModel)
        {
            try
            {
                _logger.LogInformation("Entering in ProductAPI ProductController PutProduct method");
                if (id != cartModel.ShoppingCartId)
                {
                    _logger.LogInformation("In ProductAPI ProductController PutProduct method, Id not found");
                    return NotFound();
                }
                var cart = await _repo.UpdateCart(cartModel);
                _logger.LogInformation("Exiting from ProductAPI ProductController PutProduct method Successfully");
                return cart;
            }
            catch (Exception e)
            {
                _logger.LogError("Exiting from ProductAPI ProductController PutProduct method, Exception Occured" + e.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// This function is used to reduce the product quantity by 1 on removing the item from the cart
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shoppingCartModel"></param>
        /// <returns></returns>
        [HttpPut("Product/{id}")]
        public async Task<ActionResult<ProductModel>> UpdateProductQuantity(int id,  ShoppingCartModel shoppingCartModel)
        {
            try
            {
                _logger.LogInformation("Entering in ProductAPI ProductController PutProduct method");
                if (id == null)
                {
                    _logger.LogInformation("In ProductAPI ProductController PutProduct method, Id not found");
                    return NotFound();
                }
                int qty = shoppingCartModel.ProductQty;
                var cart = await _repo.UpdateProductQuantity(id,qty);

                _logger.LogInformation("Exiting from ProductAPI ProductController PutProduct method Successfully");
                return cart;
            }
            catch (Exception e)
            {
                _logger.LogError("Exiting from ProductAPI ProductController PutProduct method, Exception Occured" + e.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// This function is used to remove the entire Cart Id based on the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task DeleteCart(int id)
        {
            try
            {
                _logger.LogInformation("Entering in ProductAPI ProductController DeleteProduct method");
                await _repo.DeleteCart(id);
                _logger.LogInformation("Exiting from ProductAPI ProductController PutProduct method");
                return;
            }
            catch (Exception e)
            {
                _logger.LogError("Exiting from ProductAPI ProductController DeleteProduct method, Exception Occured" + e.Message);
                return;
            }
        }
    }
}