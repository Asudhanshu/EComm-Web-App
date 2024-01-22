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
    /// <summary>
    /// This is the Category Controller of CategoryApi which will be taking the data from Repository Class in Business Acess Layer
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        //Property
        private readonly ICategoryRepo _repo;
        private readonly ILogger<CategoriesController> _logger;
        /// <summary>
        /// Constructor to initialize the ICategoryRepo
        /// </summary>
        /// <param name="repo"></param>
        public CategoriesController(ICategoryRepo repo, ILogger<CategoriesController> logger)
        {
            _logger = logger;
            _repo = repo;
        }
        // GET: api/GetCategory/{pg}
        /// <summary>
        /// To get the list of Categories from the Repository Using Paging
        /// </summary>
        /// <returns></returns>
        /// Authorized based on the JWT token
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetCategory/{pg}")]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategories(int pg=1)
        {
            try
            {
                return await _repo.GetCategories(pg);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        /// <summary>
        /// To Get the count of records present in Database from Business Logic Layer(Repository)
        /// </summary>
        [HttpGet("GetCount")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<int> GetCategoryCount()
        {
            var getCount = await _repo.GetCategoryCount();
            return getCount;
        }
        // GET: api/Categorys/5
        /// <summary>
        /// This function is used to get the Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CategoryModel>> GetCategory(int id)
        {
            try
            {
                _logger.LogInformation("Entering in CategoryAPI CategoryController GetCategory by Id method");
                var category = await _repo.GetCategory(id);
                if (category == null)
                {
                    _logger.LogInformation("Exiting from CategoryAPI CategoryController GetCategory by Id because no Category found");
                    return NotFound();
                }
                _logger.LogInformation("Exiting from CategoryAPI CategoryController GetCategory by Id method Successfully");
                return category;
            }
            catch (Exception e)
            {
                _logger.LogError("Got the error in CategoryAPI CategoryController while fetching the data from repository" + e.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// This function is used to Add the new Category into repository
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CategoryModel>> PostCategory(CategoryModel categoryModel)
        {
            try
            {
                _logger.LogInformation("Entering in CategoryAPI CategoryController PostCategory method");
                await _repo.AddCategory(categoryModel);
                _logger.LogInformation("Exiting from CategoryAPI CategoryController PostCategory method");
                return CreatedAtAction("GetCategory", new { id = categoryModel.CategoryId }, categoryModel);
            }
            catch (Exception e)
            {
                _logger.LogError("Got the error in CategoryAPI CategoryController while creating the data into repository" + e.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// This function is used to update the Categories based on the Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CategoryModel>> PutCategory(int id, CategoryModel categoryModel)
        {
            try
            {
                _logger.LogInformation("Entering in CategoryAPI CategoryController PutCategory method");
                if (id != categoryModel.CategoryId)
                {
                    _logger.LogInformation("In CategoryAPI CategoryController PutCategory method, Id not found");
                    return NotFound();
                }
                var Category = await _repo.UpdateCategory(categoryModel);
                _logger.LogInformation("Exiting from CategoryAPI CategoryController PutCategory method Successfully");
                return Category;
            }
            catch (Exception e)
            {
                _logger.LogError("Exiting from CategoryAPI CategoryController PutCategory method, Exception Occured" + e.Message);
                return BadRequest();
            }
        }
        //// DELETE: api/Categorys/5
        /// <summary>
        /// This function is used to delete the Category based on the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task DeleteCategory(int id)
        {
            try
            {
                _logger.LogInformation("Entering in CategoryAPI CategoryController DeleteCategory method");
                await _repo.DeleteCategory(id);
                _logger.LogInformation("Exiting from CategoryAPI CategoryController PutCategory method");
                return;
            }
            catch (Exception e)
            {
                _logger.LogError("Exiting from CategoryAPI CategoryController DeleteCategory method, Exception Occured" + e.Message);
                return;
            }
        }
    }
}
