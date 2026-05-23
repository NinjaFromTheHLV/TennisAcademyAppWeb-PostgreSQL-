using Microsoft.AspNetCore.Mvc;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.Coach;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Coach;
using TennisAcademyApp.Services.Core.Contracts;

namespace TennisAcademyApp.Controllers
{
    public class FavouriteCoachController : BaseController
    {
        private readonly IFavouriteCoachService favouriteCoachService;
        public FavouriteCoachController(IFavouriteCoachService coachService)
        {
            this.favouriteCoachService = coachService;
        }
        [HttpGet]
        public async Task<IActionResult> Favourite()
        {
            try
            {
                string userId = GetUserId()!;

                var favourites = await this.favouriteCoachService.GetFavouritesAsync(userId);

                return View(favourites);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction(nameof(Index), "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddToFavourites(int id)
        {
            try
            {
                string userId = GetUserId()!;

                await favouriteCoachService.AddFavouriteCoachAsync(userId, id);
                TempData["SuccessMessage"] = CoachFavouriteAddedSuccessfully;

                return RedirectToAction(nameof(Favourite));
            }
            catch (ArgumentException)
            {
                TempData["ErrorMessage"] = CoachAlreadyAddedToFavouritesErrorMessage;
                return RedirectToAction(nameof(Index), "Coach");
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromFavourites(int id)
        {
            try
            {
                var userId = GetUserId()!;

                await favouriteCoachService.RemoveFromFavouritesAsync(userId, id);
                TempData["SuccessMessage"] = CoachFavouriteRemovedSuccessfully;

                return RedirectToAction(nameof(Favourite));
            }
            catch (ArgumentException)
            {
                TempData["ErrorMessage"] = CoachNotFoundErrorMessage;
                return RedirectToAction(nameof(Favourite));
            }
        }
    }
}
