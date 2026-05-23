using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages;

namespace TennisAcademyApp.Controllers
{
    public class BallController : BaseController
    {
        private readonly IBallService ballService;

        public BallController(IBallService ballService)
        {
            this.ballService = ballService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                var balls = await this.ballService.GetAllBallsAsync();
                return View(balls);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = UnexpectedError;
                return RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
