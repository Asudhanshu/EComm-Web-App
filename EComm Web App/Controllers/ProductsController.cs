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
    [Authorize]
    public class ProductsController : Controller
    {
        //Property of ILogger type to initialize the logger
        private readonly ILogger<ProductsController> _logger;
       
        private readonly IWebHostEnvironment _hostEnvironment;


        APIProductAPIUrl _APIUrl = new APIProductAPIUrl();
        /// <summary>
        /// Constructor to Initialize Logging and Connect our EComm to API
        /// </summary>
        /// <param name="logger"></param>
        /// 
        public ProductsController(ILogger<ProductsController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }
       
        /// <summary>
        /// Index Function Consist Information for EComm Index page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(int pg=1)
        {
            _logger.LogInformation("Product Controller Index method called");
            List<ProductModel> products = new List<ProductModel>();
            int count=0;
            APIProductAPIUrl URL = new APIProductAPIUrl();
            HttpClient client = URL.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync("api/Products/GetProduct/"+pg);
            var JwtToken1 = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken1);
            HttpResponseMessage response2 = await client.GetAsync("api/Products/GetCount");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<List<ProductModel>>(result);
            }
            if (response2.IsSuccessStatusCode)
            {
                var result = response2.Content.ReadAsStringAsync().Result;
                count = JsonConvert.DeserializeObject<int>(result);
            }
            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = count;
            var pager = new Pager(recsCount,pg,pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = products.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager=pager;
            //return View(data);
            return View(products);
        }

        //GET: Products/Create
        /// <summary>
        /// Create Function to Add the new Products using API. 
        /// </summary>
        /// <returns></returns>        
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("In EComm Project Product Controller Entering into Non-Parameterized Create Method");
            //Creating the List of ProductModel type to get the list of Products available in Database through API
            List<CategoryModel> categories = new List<CategoryModel>();
            //Connecting to API and fetching the data from API
            HttpClient client = _APIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync("api/products/category");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<CategoryModel>>(result);
            }

            ViewBag.Categories = categories;
            _logger.LogInformation("In EComm Project Product Controller Exiting from Create Method Successfully");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductBrand,ProductDescription,ProductPrice,CategoryId,CreatedOn,ModifiedOn,ModifiedBy,ProductQty,Image,ImageFile")] ProductModel productview)
        {
            //Save image to image folder
            string pname = productview.ProductName;
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(productview.ImageFile.FileName);
            string extension = Path.GetExtension(productview.ImageFile.FileName);
            productview.Image = fileName = pname + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/Image/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await productview.ImageFile.CopyToAsync(fileStream);
            }


            _logger.LogInformation("In EComm Project Product Controller Entering into Create Method for binding the data");
            HttpClient client = _APIUrl.Initial();
            productview.ModifiedBy = User.Identity.Name;
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            var insert = await client.PostAsync("api/products/product", new StringContent(JsonConvert.SerializeObject(productview), Encoding.UTF8, "application/json"));
            if(insert.IsSuccessStatusCode)
            {
                _logger.LogInformation($"admin {User.Identity.Name} successfully created a new product with name => {productview.ProductName}");
            }
            _logger.LogInformation("In EComm Project Product Controller Exiting from Create Method after Successfully Binding");
            return RedirectToAction(nameof(Index));
        }
        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("In EComm Project Product Controller Entering into Edit Method of nullable parameter");
            if (id == null)
            {
                _logger.LogWarning("In EComm Project Product Controller Entering into Edit Method if condition as no Product is found");
                return NotFound();
            }
            ProductModel products = new ProductModel();
            HttpClient client = _APIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync("api/products/" + id);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("In EComm Project Product Controller Entering into Edit Method if Condition to deserialize the data");
                var result = response.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<ProductModel>(result);
            }

            ViewBag.Product = products;

            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductBrand,ProductDescription,ProductPrice,CategoryId,CreatedOn,ModifiedOn,ModifiedBy,ProductQty,Image")] ProductModel productview)
        {
            //
            if (id != productview.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    HttpClient client = _APIUrl.Initial();
                    productview.ModifiedBy = User.Identity.Name;
                    var JwtToken = HttpContext.Session.GetString("JwtToken");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
                    var insert = await client.PutAsync("api/products/" + id, new StringContent(JsonConvert.SerializeObject(productview), Encoding.UTF8, "application/json"));
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

            }
            return View(productview);
        }
        public async Task<IActionResult> Details(int? Id)
        {
            ProductModel products = new ProductModel();
            HttpClient client = _APIUrl.Initial();
            // _client = _ProductAPIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync($"api/products/{Id}");
            _logger.LogInformation("This is log trace - Detail method called");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<ProductModel>(result);
            }
            return View(products);
        }
        //// POST: Employees/Delete/1

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? Id)
        {
            ProductModel products = new ProductModel();

            HttpClient client = _APIUrl.Initial();

            // HttpResponseMessage response = await client.DeleteAsync($"api/Products/{Id}");
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync($"api/products/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<ProductModel>(result);
                
            }
            return View(products);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel products = new ProductModel();

            HttpClient client = _APIUrl.Initial();

            var JwtToken1 = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken1);

            HttpResponseMessage response1 = await client.GetAsync($"api/products/{Id}");

            if (response1.IsSuccessStatusCode)
            {
                var result = response1.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<ProductModel>(result);
                _logger.LogInformation($"admin {User.Identity.Name} delete the product with name=> {products.ProductName}");

            }
            HttpResponseMessage response = await client.GetAsync($"api/products/{Id}");
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            var delete = await client.DeleteAsync($"api/products/{Id}");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string searchString)
        {
            List<ProductModel> products = new List<ProductModel>();
            bool isProductFound = false;
            HttpClient client = _APIUrl.Initial();
            Login login = new Login();

            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync("api/products/GetProduct");
            if (searchString == null)
            {
                ModelState.AddModelError("", "Please enter some data");
                ViewBag.isProductFound = false;
                return View();
            }
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<List<ProductModel>>(result);
            }
            List<ProductModel> returnProduct = new List<ProductModel>();
            foreach (ProductModel product in products)
            {
                if (product.ProductName.ToLower().Contains(searchString.ToLower()))
                {
                    returnProduct.Add(product);
                    isProductFound = true;
                    ViewBag.isProductFound = isProductFound;
                }
            }
            if (returnProduct.Count == 0)
            {
                isProductFound = false;
                ViewBag.isProductFound = isProductFound;
                ModelState.AddModelError("", "No Product found with given title");
            }
            return View(returnProduct);
        }



        //Add to Cart Functionality
    }
}
