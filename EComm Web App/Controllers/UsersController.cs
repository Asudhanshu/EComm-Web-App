using EComm_Web_App.Helper;
using EComm_Web_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EComm_Web_App.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        APIProductAPIUrl _APIUrl = new APIProductAPIUrl();

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;

        }


        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("");
            List<UsersModel> user = new List<UsersModel>();
            try
            {
                //Connecting to API and fetching the data from API
                HttpClient client = _APIUrl.Initial();
                HttpResponseMessage response = await client.GetAsync("api/users");
                if (response.IsSuccessStatusCode)
                {
                    //Fetching the data from API
                    var result = response.Content.ReadAsStringAsync().Result;
                    //Deserializing the Data from JSON to list of BooKModel
                    user = JsonConvert.DeserializeObject<List<UsersModel>>(result);
                }
                _logger.LogInformation("");
                return View(user);
            }
            catch (Exception e)
            {
                _logger.LogError("");
                return View("Index");
            }
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("");
            List<UsersModel> user = new List<UsersModel>();
            HttpClient client = _APIUrl.Initial();
            HttpResponseMessage response = await client.GetAsync("api/users");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<List<UsersModel>>(result);
            }

            ViewBag.Book = user;
            _logger.LogInformation("");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("UserId,AspUserId,FirstName,LastName,Address,Mobile,Email,CreatedOn")] UsersModel userview)
        {
            _logger.LogInformation("");
            HttpClient client = _APIUrl.Initial();
            var insert = await client.PostAsync("api/users", new StringContent(JsonConvert.SerializeObject(userview), Encoding.UTF8, "application/json"));
            _logger.LogInformation("");
            return RedirectToAction(nameof(Index));
        }



        //public async Task<IActionResult> Edit(int? id)
        //{
        //    _logger.LogInformation("");
        //    if (id == null)
        //    {
        //        _logger.LogWarning("In LMS Project Book Controller Entering into Edit Method if condition as no book is found");
        //        return NotFound();
        //    }
        //    CategoryModel category = new CategoryModel();
        //    HttpClient client = _APIUrl.Initial();
        //    HttpResponseMessage response = await client.GetAsync("api/categories/" + id);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        _logger.LogInformation("In LMS Project Book Controller Entering into Edit Method if Condition to deserialize the data");
        //        var result = response.Content.ReadAsStringAsync().Result;
        //        category = JsonConvert.DeserializeObject<CategoryModel>(result);
        //    }
        //    List<CategoryModel> categories = new List<CategoryModel>();
        //    HttpResponseMessage bookresponse = await client.GetAsync("api/categories");

        //    if (bookresponse.IsSuccessStatusCode)
        //    {
        //        var result = bookresponse.Content.ReadAsStringAsync().Result;
        //        categories = JsonConvert.DeserializeObject<List<CategoryModel>>(result);
        //    }

        //    ViewBag.Book = categories;

        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(category);
        //}



        // POST: Books/Edit/5
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





        public async Task<IActionResult> Detail(int? Id)
        {
            CategoryModel categories = new CategoryModel();
            HttpClient client = _APIUrl.Initial();
            // _client = _bookAPIUrl.Initial();
            HttpResponseMessage response = await client.GetAsync($"api/categories/{Id}");
            _logger.LogWarning("This is log trace - Detail method called");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<CategoryModel>(result);
            }
            return View(categories);
        }

        //// POST: BookController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try

        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Employees/Delete/1
        //public async Task<IActionResult> Delete(int? Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }
        //    var book = await _dbContext.books.FirstOrDefaultAsync(m => m.Id == Id);

        //    return View(book);
        //}

        //// POST: Employees/Delete/1

        public async Task<IActionResult> Delete(int? Id)
        {
            CategoryModel categories = new CategoryModel();

            HttpClient client = _APIUrl.Initial();

            // HttpResponseMessage response = await client.DeleteAsync($"api/books/{Id}");

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
            var delete = await client.DeleteAsync($"api/categories/{Id}");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string searchString)
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            bool isProductFound = false;
            HttpClient client = _APIUrl.Initial();
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
            List<CategoryModel> returnBook = new List<CategoryModel>();
            foreach (CategoryModel category in categories)
            {
                if (category.CategoryName.ToLower().Contains(searchString.ToLower()))
                {
                    returnBook.Add(category);
                    isProductFound = true;
                    ViewBag.isBookFound = isProductFound;
                }
            }
            if (returnBook.Count == 0)
            {
                isProductFound = false;
                ViewBag.isBookFound = isProductFound;
                ModelState.AddModelError("", "No Product found with given title");
            }
            return View(returnBook);
        }
    }
}
