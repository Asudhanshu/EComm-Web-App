using EComm_Web_App.Helper;
using EComm_Web_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace EComm_Web_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        //Property of ILogger type to initialize the logger
        private readonly ILogger<CategoryController> _logger;
        APIProductAPIUrl _APIUrl = new APIProductAPIUrl();
        /// <summary>
        /// Constructor to Initialize Logging and Connect our ECommerce to API
        /// </summary>
        /// <param name="logger"></param>
        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Index Function Consist Information for Ecomm Index page which will display all the CRUD Operations
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(int pg=1)
        {
            int count = 0;
            _logger.LogInformation("In EComm Project Category Controller Entering into Index Method");
            //Creating the List of CategoryModel type to get the list of Categories available in Database through API
            List<CategoryModel> category = new List<CategoryModel>();
            try
            {
                //Connecting to API and fetching the data from API
                HttpClient client = _APIUrl.Initial();
                var JwtToken = HttpContext.Session.GetString("JwtToken");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
                HttpResponseMessage response = await client.GetAsync("api/categories/GetCategory/"+pg);
                var JwtToken1 = HttpContext.Session.GetString("JwtToken");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken1);
                HttpResponseMessage response2 = await client.GetAsync("api/Categories/GetCount");
                if (response.IsSuccessStatusCode)
                {
                    //Fetching the data from API
                    var result = response.Content.ReadAsStringAsync().Result;
                    //Deserializing the Data from JSON to list of CategoryModel
                    category = JsonConvert.DeserializeObject<List<CategoryModel>>(result);
                }
                else
                {
                    int httpcode = (int)response.StatusCode;
                    ErrorPageController.statuscode = httpcode;
                    return RedirectToAction("ErrorPage", "ErrorPage");
                }

                if (response2.IsSuccessStatusCode)
                {
                    var result = response2.Content.ReadAsStringAsync().Result;
                    count = JsonConvert.DeserializeObject<int>(result);
                }
                _logger.LogInformation("In EComm Project Category Controller Exiting from Index Method Successfully");

                const int pageSize = 5;
                if (pg < 1)
                {
                    pg = 1;
                }
                int recsCount = count;
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = category.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;

                return View(category);
            }
            catch (Exception e)
            {
                _logger.LogError("In EComm Project Category Controller couldn't exit from Index Method, Got the error" + e.Message);
                return View("Index");
            }
        }
        // GET: Categorys/Create
        /// <summary>
        /// Create Function to Add the new Categorys using API. 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("In EComm Project Category Controller Entering into Non-Parameterized Create Method");
            //Creating the List of CategoryModel type to get the list of Categorys available in Database through API
            List<CategoryModel> category = new List<CategoryModel>();
            //Connecting to API and fetching the data from API
            HttpClient client = _APIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync("api/categories");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<List<CategoryModel>>(result);
            }

            ViewBag.Category = category;
            _logger.LogInformation("In EComm Project Category Controller Exiting from Create Method Successfully");
            return View();
        }
        // POST: Categorys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,CategoryDescription,CreatedOn,ModifiedOn,ModifiedBy")] CategoryModel categoryview)
        {
            _logger.LogInformation("In EComm Project Category Controller Entering into Create Method for binding the data");
            HttpClient client = _APIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            var insert = await client.PostAsync("api/categories", new StringContent(JsonConvert.SerializeObject(categoryview), Encoding.UTF8, "application/json"));
            _logger.LogInformation("In EComm Project Category Controller Exiting from Create Method after Successfully Binding");
            return RedirectToAction(nameof(Index));
        }
        // GET: Categorys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("In EComm Project Category Controller Entering into Edit Method of nullable parameter");
            if (id == null)
            {
                _logger.LogWarning("In EComm Project Category Controller Entering into Edit Method if condition as no Category is found");
                return NotFound();
            }
            CategoryModel category = new CategoryModel();
            HttpClient client = _APIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync("api/categories/" + id);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("In EComm Project Category Controller Entering into Edit Method if Condition to deserialize the data");
                var result = response.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<CategoryModel>(result);
            }
            List<CategoryModel> categories = new List<CategoryModel>();
            var JwtToken1 = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken1);
            HttpResponseMessage Categoryresponse = await client.GetAsync("api/categories");

            if (Categoryresponse.IsSuccessStatusCode)
            {
                var result = Categoryresponse.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<CategoryModel>>(result);
            }

            ViewBag.Category = categories;

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        // POST: Categorys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,CategoryDescription,CreatedOn,ModifiedOn,ModifiedBy")] CategoryModel categoryview)
        {
            if (id != categoryview.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    HttpClient client = _APIUrl.Initial();
                    var JwtToken = HttpContext.Session.GetString("JwtToken");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
                    var insert = await client.PutAsync("api/categories/" + id, new StringContent(JsonConvert.SerializeObject(categoryview), Encoding.UTF8, "application/json"));


                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoryview);
        }
        public async Task<IActionResult> Details(int? Id)
        {
            CategoryModel categories = new CategoryModel();
            HttpClient client = _APIUrl.Initial();
            // _client = _CategoryAPIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync($"api/categories/{Id}");
            _logger.LogWarning("This is log trace - Detail method called");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<CategoryModel>(result);
            }
            return View(categories);
        }
        public async Task<IActionResult> Delete(int? Id)
        {
            CategoryModel categories = new CategoryModel();

            HttpClient client = _APIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync($"api/categories/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<CategoryModel>(result);
            }
            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            HttpClient client = _APIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            var delete = await client.DeleteAsync($"api/categories/{Id}");

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Search(string searchString)
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            bool isProductFound = false;
            HttpClient client = _APIUrl.Initial();
            var JwtToken = HttpContext.Session.GetString("JwtToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            HttpResponseMessage response = await client.GetAsync("api/categories");
            if (searchString == null)
            {
                ModelState.AddModelError("", "Please enter some data");
                ViewBag.isProductFound = false;
                return View();
            }
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<CategoryModel>>(result);
            }
            List<CategoryModel> returnCategory = new List<CategoryModel>();
            foreach (CategoryModel category in categories)
            {
                if (category.CategoryName.ToLower().Contains(searchString.ToLower()))
                {
                    returnCategory.Add(category);
                    isProductFound = true;
                    ViewBag.isCategoryFound = isProductFound;
                }
            }
            if (returnCategory.Count == 0)
            {
                isProductFound = false;
                ViewBag.isCategoryFound = isProductFound;
                ModelState.AddModelError("", "No Product found with given title");
            }
            return View(returnCategory);
        }
    }
}
