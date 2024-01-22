using EComm_Web_App.Helper;
using EComm_Web_App.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EComm_Web_App.Controllers
{
    public class BulkUploadController : Controller
    {
        private readonly ILogger<BulkUploadController> _logger;
        APIProductAPIUrl _APIUrl = new APIProductAPIUrl();
        /// <summary>
        /// Constructor to Initialize Logging and Connect our EComm to API
        /// </summary>
        /// <param name="logger"></param>
        /// 
        public BulkUploadController(ILogger<BulkUploadController> logger)
        {
            _logger = logger;

        }
        [HttpGet]
        public IActionResult Excel(List<ProductModel> products = null)
        {
            products = products == null ? new List<ProductModel>() : products;
            return View(products);
        }
        [HttpPost]
        public async Task<IActionResult> Excel(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            var products = await this.GetProductList(file.FileName);
            return Excel(products);
        }
        private async Task<List<ProductModel>> GetProductList(string fName)
        {
            List<ProductModel> products = new List<ProductModel>();
            ProductModel product = new ProductModel();
            var fileName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            HttpClient client = _APIUrl.Initial();
            int count = 0;
            using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        products.Add(new ProductModel()
                        {
                            ProductId = 0,
                            ProductName = reader.GetValue(0).ToString(),
                            ProductBrand = reader.GetValue(1).ToString(),
                            ProductDescription = reader.GetValue(2).ToString(),
                            ProductPrice = int.Parse(reader.GetValue(3).ToString()),
                            CategoryId = int.Parse(reader.GetValue(5).ToString()),
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now,
                            ModifiedBy = User.Identity.Name,
                            ProductQty = int.Parse(reader.GetValue(4).ToString())
                        });
                        ProductModel product1 = new ProductModel();
                        product1 = products[count];
                        var JwtToken = HttpContext.Session.GetString("JwtToken");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
                        var insert = await client.PostAsync("api/products/product", new StringContent(JsonConvert.SerializeObject(product1), Encoding.UTF8, "application/json"));
                        count++;
                    }
                }
                return products;
            }
        }
    }
}
