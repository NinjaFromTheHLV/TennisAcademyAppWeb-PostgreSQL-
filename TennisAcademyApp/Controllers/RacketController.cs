using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Racket;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Racket;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.Racket;

namespace TennisAcademyApp.Controllers
{
    public class RacketController : BaseController
    {
        private readonly IRacketService racketService;

        public RacketController(IRacketService racketService)
        {
            this.racketService = racketService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                var rackets = await this.racketService.GetAllRacketsAsync();
                return View(rackets);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = UnexpectedError;
                return RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
