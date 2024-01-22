using Microsoft.AspNetCore.Mvc;

namespace EComm_Web_App.Controllers
{
    [Route("ErrorPage/{statuscode}")]
    public class ErrorPageController : Controller
    {
        public static int statuscode=404;
        public IActionResult ErrorPage()
        {
            switch (statuscode)
            {
                case 404:
                    ViewData["Error"] = "Page Not Found";
                    break;
                case 401:
                    ViewData["Error"] = "You are not the authorized User";
                    break;
                default:
                    break;
            }
            return View("ErrorPage");
        }
    }
}
