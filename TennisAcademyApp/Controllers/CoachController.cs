using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Coach;


namespace TennisAcademyApp.Controllers
{
    public class CoachController : BaseController
    {
        private readonly ICoachService coachService;
        public CoachController(ICoachService coachService)
        {
            this.coachService = coachService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchQuery = null, int page = 1, int pageSize = 3)
        {
            try
            {
                var model = await coachService.GetCoachesByPageAsync(searchQuery, page, pageSize);

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                string userId = GetUserId()!;

                var details = await this.coachService.GetCoachDetailsAsync(userId, id);

                return View(details);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
    }
}
