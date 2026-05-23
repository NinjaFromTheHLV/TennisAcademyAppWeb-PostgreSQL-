using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Bag;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.Bag;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Bag;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages;

namespace TennisAcademyApp.Controllers
{
    public class BagController : BaseController
    {
        private readonly IBagService bagService;

        public BagController(IBagService bagService)
        {
            this.bagService = bagService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                var bags = await this.bagService.GetAllBagsAsync();
                return View(bags);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = UnexpectedError;
                return RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
