using EComm_Web_App.Helper;
using EComm_Web_App.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace EComm_Web_App.Controllers
{
    [Authorize(Roles ="Users")]
    public class ShoppingController : Controller
    {
        private readonly ILogger<ShoppingController> _logger;
        APIProductAPIUrl _APIUrl = new APIProductAPIUrl();
        /// <summary>
        /// Constructor to Initialize Logging and Connect our EComm to API
        /// </summary>
        /// <param name="logger"></param>
        /// 
        public ShoppingController(ILogger<ShoppingController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> AddToCart(int? id)
        {
            _logger.LogInformation("In EComm Project Product Controller Entering into Edit Method of nullable parameter");
            if (id == null)
            {
                _logger.LogWarning("In EComm Project Product Controller Entering into Edit Method if condition as no Product is found");
                return NotFound();
            }
            ProductModel product = new ProductModel();
            HttpClient client = _APIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync("api/products/" + id);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("In EComm Project Product Controller Entering into Edit Method if Condition to deserialize the data");
                var result = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<ProductModel>(result);
            }
            List<ShoppingCartModel> ListOfCart = new List<ShoppingCartModel>();
            var JwtToken1 = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken1);
            HttpResponseMessage response2 = await client.GetAsync($"api/Shopping/GetCart/{User.Identity.Name}");
            if (response.IsSuccessStatusCode)
            {
                var result2 = response2.Content.ReadAsStringAsync().Result;
                ListOfCart = JsonConvert.DeserializeObject<List<ShoppingCartModel>>(result2);
            }
            int passId = 0;
            int qty = 1;
            foreach (var items in ListOfCart)
            {
                if ((items.ProductId == product.ProductId) && (User.Identity.Name.Equals(items.UserName)))
                {
                    passId = items.ShoppingCartId;
                    qty = items.ProductQty;
                }
                //else if(items.ProductId == product.ProductId)
                //{
                //    //passId = items.ShoppingCartId;
                //    qty = items.ProductQty;
                //}
            }
            if (product.ProductQty <= 0)
            {
                // error message
                return RedirectToAction("Index", "Products");
            }
            else
            {
                ShoppingCartModel shoppingview = new ShoppingCartModel();
                shoppingview.ProductQty = qty;
                shoppingview.ProductId = product.ProductId;
                shoppingview.ProductName = product.ProductName;
                shoppingview.ProductPrice = product.ProductPrice;
                shoppingview.UserName = User.Identity.Name;
                shoppingview.ShoppingCartId = passId;
                var JwtToken2 = HttpContext.Session.GetString("JwtToken");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken2);
                var insert = await client.PostAsync("api/Shopping/ShoppingCart", new StringContent(JsonConvert.SerializeObject(shoppingview), Encoding.UTF8, "application/json"));
                if (product == null)
                {
                    return NotFound();
                }
                return RedirectToAction("Index", "Products");
            }
        }

        public async Task<IActionResult> ViewCart()
        {
            bool isCartEmpty = true;
            _logger.LogInformation("Add Product Controller Index method called");
            List<ShoppingCartModel> shoppingCartModels = new List<ShoppingCartModel>();
            APIProductAPIUrl URL = new APIProductAPIUrl();
            HttpClient client = URL.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync($"api/Shopping/GetCart/{User.Identity.Name}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                shoppingCartModels = JsonConvert.DeserializeObject<List<ShoppingCartModel>>(result);
            }
            if(shoppingCartModels.Count == 0)
            {
                ModelState.AddModelError("", "Your Cart is Currently Empty");
                ViewBag.isCartEmpty = true;
            }
            else
            {
                ViewBag.isCartEmpty = false;
            }
            return View(shoppingCartModels);
            //return View(products);
        }

        public async Task<IActionResult> DeleteCart(int? Id)
        {
            HttpClient client = _APIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            var delete = await client.DeleteAsync($"api/Shopping/{Id}");
            return RedirectToAction(nameof(ViewCart));
        }

        public async Task<IActionResult> Checkout()
        {
            bool isProductAvailable = true;
            int ProductQuantity=0;
            string productsName = "";
            int AskedQuantity = 0;
            _logger.LogInformation("Add Product Controller Index method called");
            List<CheckoutModel> checkoutModels = new List<CheckoutModel>();
            APIProductAPIUrl URL = new APIProductAPIUrl();
            HttpClient client = URL.Initial();
            List<ShoppingCartModel> shoppingCartModels = new List<ShoppingCartModel>();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync($"api/Shopping/GetCart/{User.Identity.Name}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                checkoutModels = JsonConvert.DeserializeObject<List<CheckoutModel>>(result);
                shoppingCartModels = JsonConvert.DeserializeObject<List<ShoppingCartModel>>(result);
            }
            int bill = 0;
            foreach (var item in checkoutModels)
            {
                item.TotalPrice = item.ProductQty * item.ProductPrice;
                bill = bill + item.TotalPrice;
                item.FinalPrice = bill;
            }

            // qty check if less breaking the flow
            foreach (var item in shoppingCartModels)
            {
                var JwtToken1 = HttpContext.Session.GetString("JwtToken");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken1);
                HttpResponseMessage response2 = await client.GetAsync("api/products/" + item.ProductId);
                ProductModel products = new ProductModel();
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("In EComm Project Product Controller Entering into Edit Method if Condition to deserialize the data");
                    var result = response2.Content.ReadAsStringAsync().Result;
                    products = JsonConvert.DeserializeObject<ProductModel>(result);
                }
                if (item.ProductQty > products.ProductQty)
                {
                    ModelState.AddModelError("", "Product is not in Stock");
                    isProductAvailable = false;
                    ViewBag.isProductAvailable = false;
                    ViewBag.ProductQuantity = products.ProductQty;
                    ViewBag.AskedQuantity = item.ProductQty;
                    ViewBag.productsName = products.ProductName;
                }
            }
            if (isProductAvailable)
            {
                ViewBag.IsProductAvailable = true;
            }
            ViewBag.FinalPrice = bill;
            return View(checkoutModels);
            //return View(products);
        }


        public async Task<IActionResult> RemoveCart(int? Id)
        {
            _logger.LogInformation("In EComm Project Product Controller Entering into Edit Method of nullable parameter");
            if (Id == null)
            {
                _logger.LogWarning("In EComm Project Product Controller Entering into Edit Method if condition as no Product is found");
                return NotFound();
            }
            ShoppingCartModel cart = new ShoppingCartModel();
            HttpClient client = _APIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync($"api/Shopping/{Id}");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("In EComm Project Product Controller Entering into Edit Method if Condition to deserialize the data");
                var result = response.Content.ReadAsStringAsync().Result;
                cart = JsonConvert.DeserializeObject<ShoppingCartModel>(result);
            }

            if (cart.ProductQty > 1)
            {
                var JwtToken1 = HttpContext.Session.GetString("JwtToken");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken1);
                var remove = await client.PutAsync("api/shopping/" + Id, new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json"));
            }
            else
            {
                var JwtToken2 = HttpContext.Session.GetString("JwtToken");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken2);
                var delete = await client.DeleteAsync($"api/Shopping/{Id}");
                return RedirectToAction(nameof(ViewCart));
            }
            return RedirectToAction(nameof(ViewCart));
        }
        public async Task<IActionResult> OrderItem()
        {
            _logger.LogInformation("Add Product Controller Index method called");
            List<ShoppingCartModel> shoppingCartModels = new List<ShoppingCartModel>();
            APIProductAPIUrl URL = new APIProductAPIUrl();
            HttpClient client = URL.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync($"api/Shopping/GetCart/{User.Identity.Name}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                shoppingCartModels = JsonConvert.DeserializeObject<List<ShoppingCartModel>>(result);
            }
            int totalBill = 0;
            int temp;
            foreach(var items in shoppingCartModels)
            {
                temp = items.ProductPrice * items.ProductQty;
                totalBill = totalBill + temp;
            }
            OrderModel orderView = new OrderModel();
            _logger.LogInformation("In EComm Project Product Controller Entering into Create Method for binding the data");
            //HttpClient client = _APIUrl.Initial();
            orderView.OrderId = 0;
            orderView.UserName = User.Identity.Name;
            orderView.OrderDate = DateTime.Now;
            orderView.TotalPrice = totalBill;
            var JwtToken1 = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken1);
            var insert = await client.PostAsync("api/Shopping/order", new StringContent(JsonConvert.SerializeObject(orderView), Encoding.UTF8, "application/json"));
            _logger.LogInformation("In EComm Project Product Controller Exiting from Create Method after Successfully Binding");
            if(insert.IsSuccessStatusCode)
            {
                foreach (var items in shoppingCartModels)
                {
                    int productID = items.ProductId;
                    //int Qty = items.ProductQty;
                    var JwtToken2 = HttpContext.Session.GetString("JwtToken");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken2);
                    var remove = await client.PutAsync("api/shopping/product/" + productID, new StringContent(JsonConvert.SerializeObject(items), Encoding.UTF8, "application/json"));
                    var JwtToken3 = HttpContext.Session.GetString("JwtToken");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken3);
                    var delete = await client.DeleteAsync($"api/Shopping/{items.ShoppingCartId}");

                }
            }
            return View();
        }

        public async Task<IActionResult> ViewOrderHistory()
        {
            _logger.LogInformation("Add Product Controller Index method called");
            List<OrderModel> orderModels = new List<OrderModel>();
            APIProductAPIUrl URL = new APIProductAPIUrl();
            HttpClient client = URL.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync($"api/Shopping/GetOrder/{User.Identity.Name}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                orderModels = JsonConvert.DeserializeObject<List<OrderModel>>(result);
            }

            return View(orderModels);
            //return View(products);
        }

    }
}
