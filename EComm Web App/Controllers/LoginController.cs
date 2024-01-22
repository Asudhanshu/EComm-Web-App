using EComm_Web_App.Helper;
using EComm_Web_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EComm_Web_App.Controllers
{
    public class LoginController : Controller
    {
        APIProductAPIUrl _APIUrl = new APIProductAPIUrl();

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, ILogger<LoginController> logger)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;

        }
        /// <summary>
        /// To redirect to Index page of Login View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// To redirect to Login page of Login View
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// Asynchronous LoginUser function to Login the user into Application
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<IActionResult> LoginUser(Login login)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RemeberMe, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"user => {login.Username} successfully signed in");
                    HttpClient client = _APIUrl.Initial();
                    var insert = await client.PostAsync("api/login/login", new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json"));
                    var response = insert.Content.ReadAsStringAsync().Result;
                    var userToken = JsonConvert.DeserializeObject<TokenModel>(response);
                    var jwtToken = userToken.Token.ToString();

                    HttpContext.Session.SetString("JwtToken", jwtToken); //Set

                    return RedirectToAction("Index", "Login");
                }
                _logger.LogError($"some unidentified user with email {login.Username} tried logging in access was DENIED");
                ModelState.AddModelError("", "User name or password incorrect");
            }
            return View("Login");
        }

        // GET: LoginController/Details/5
        /// <summary>
        /// To redirect to SignUp page of Login View
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// Asynchronous RegisterUser function to Register the user into Application
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public async Task<IActionResult> RegisterUser(SignUp register)
        {

            //_logger.LogInformation("In EComm Project Category Controller Entering into Create Method for binding the data");
            HttpClient client = _APIUrl.Initial();
            var insert = await client.PostAsync("api/Login/Register", new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json"));
            if (insert.IsSuccessStatusCode)
            {
                _logger.LogInformation($"new user {register.Email} is registered as {register.RoleName}");  
                return View("Login");
            }
            _logger.LogError($"new user {register.Email} tried to register as {register.RoleName} but failed");
            ModelState.AddModelError("","Username or Password Incorrect");
            
            return View("Register");
        }
        // GET: LoginController/Create
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation($"user => {User.Identity.Name} is successfully logged out");
            return RedirectToAction("Login");
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}