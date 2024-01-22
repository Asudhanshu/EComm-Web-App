using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using ECommRepo.Repository;
using Microsoft.Extensions.Logging;
using ECommRepo.Models;

namespace ECommAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //Property
        private readonly IUsersRepo _repo;
        private readonly ILogger<UsersController> _logger;
        /// <summary>
        /// Constructor to initialize the IBookRepo
        /// </summary>
        /// <param name="repo"></param>
        public UsersController(IUsersRepo repo, ILogger<UsersController> logger)
        {
            _logger = logger;
            _repo = repo;
        }

        // GET: api/Books
        /// <summary>
        /// To get the list of books from the Repository
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersModel>>> GetUsers()
        {
            try
            {
                return await _repo.GetUsers();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        // GET: api/Books/5
        /// <summary>
        /// This function is used to get the Book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UsersModel>> GetUser(int id)
        {
            try
            {
                _logger.LogInformation("Entering in BookAPI BookController GetBook by Id method");
                var user = await _repo.GetUser(id);
                if (user == null)
                {
                    _logger.LogInformation("Exiting from BookAPI BookController GetBook by Id because no book found");
                    return NotFound();
                }
                _logger.LogInformation("Exiting from BookAPI BookController GetBook by Id method Successfully");
                return user;
            }
            catch (Exception e)
            {
                _logger.LogError("Got the error in BookAPI BookController while fetching the data from repository" + e.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// This function is used to update the books based on the Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UsersModel>> PutCategory(int id, UsersModel userModel)
        {
            try
            {
                _logger.LogInformation("Entering in BookAPI BookController PutBook method");
                if (id != userModel.UserId)
                {
                    _logger.LogInformation("In BookAPI BookController PutBook method, Id not found");
                    return NotFound();
                }
                var user = await _repo.UpdateUser(userModel);
                _logger.LogInformation("Exiting from BookAPI BookController PutBook method Successfully");
                return user;
            }
            catch (Exception e)
            {
                _logger.LogError("Exiting from BookAPI BookController PutBook method, Exception Occured" + e.Message);
                return BadRequest();
            }
        }
    }
}
