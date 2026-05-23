using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace TennisAcademyApp.Controllers
{
    public class HomeController : BaseController
    {

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index), "Home", new { area = "Admin" });
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Home/Error/{statusCode}")]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode == 404)
            {
                return View("Error404");
            }

            return View("Error500");
        }
    }
}
