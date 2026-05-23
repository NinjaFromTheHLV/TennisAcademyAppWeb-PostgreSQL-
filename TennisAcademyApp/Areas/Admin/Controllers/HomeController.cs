using Microsoft.AspNetCore.Mvc;

namespace TennisAcademyApp.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
