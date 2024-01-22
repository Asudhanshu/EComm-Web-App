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
    public class ProductsController : ControllerBase
    {
        //Property
        private readonly IProductRepo _repo;
        private readonly ILogger<ProductsController> _logger;
        /// <summary>
        /// Constructor to initialize the IProductRepo
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="logger"></param>
        public ProductsController(IProductRepo repo, ILogger<ProductsController> logger)
        {
            _logger = logger;
            _repo = repo;
        }
        // GET: api/GetProduct/{pg}
        /// <summary>
        /// To get the list of Products from the Repository using Paging
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProduct/{pg}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProducts(int pg)
        {
            try
            {
                return await _repo.GetProducts(pg);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        // GET: api/GetProduct
        /// <summary>
        /// To get the complete list of Products from the Repository
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProductsList()
        {
            try
            {
                return await _repo.GetProductsList();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        /// <summary>
        /// To get the count of available number of products in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCount")]
        public async Task<int> GetProductCount()
        {
            var getCount = await _repo.GetProductCount();
            return getCount;
        }
        // GET: api/Products/5
        /// <summary>
        /// This function is used to get the Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProduct(int id)
        {
            try
            {
                _logger.LogInformation("Entering in ProductAPI ProductController GetProduct by Id method");
                var product = await _repo.GetProduct(id);
                if (product == null)
                {
                    _logger.LogInformation("Exiting from ProductAPI ProductController GetProduct by Id because no Product found");
                    return NotFound();
                }
                _logger.LogInformation("Exiting from ProductAPI ProductController GetProduct by Id method Successfully");
                return product;
            }
            catch (Exception e)
            {
                _logger.LogError("Got the error in ProductAPI ProductController while fetching the data from repository" + e.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// This function is used to get the categories
        /// </summary>
        /// <returns></returns>
        [HttpGet("category")]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategories()
        {
            return await _repo.GetCategory();
        }
        /// <summary>
        /// This function is used to Add the new Product into repository
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("Product")]
        public async Task<ActionResult<ProductModel>> PostProduct(ProductModel productModel)
        {
            try
            {
                _logger.LogInformation("Entering in ProductAPI ProductController PostProduct method");
                await _repo.AddProduct(productModel);
                _logger.LogInformation("Exiting from ProductAPI ProductController PostProduct method");
                return CreatedAtAction("GetProduct", new { id = productModel.ProductId }, productModel);
            }
            catch (Exception e)
            {
                _logger.LogError("Got the error in ProductAPI ProductController while creating the data into repository" + e.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// This function is used to update the Products based on the Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductModel>> PutCategory(int id, ProductModel productModel)
        {
            try
            {
                _logger.LogInformation("Entering in ProductAPI ProductController PutProduct method");
                if (id != productModel.ProductId)
                {
                    _logger.LogInformation("In ProductAPI ProductController PutProduct method, Id not found");
                    return NotFound();
                }
                var product = await _repo.UpdateProduct(productModel);
                _logger.LogInformation("Exiting from ProductAPI ProductController PutProduct method Successfully");
                return product;
            }
            catch (Exception e)
            {
                _logger.LogError("Exiting from ProductAPI ProductController PutProduct method, Exception Occured" + e.Message);
                return BadRequest();
            }
        }
        //// DELETE: api/Products/5
        /// <summary>
        /// This function is used to delete the Product based on the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation("Entering in ProductAPI ProductController DeleteProduct method");
                await _repo.DeleteProduct(id);
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