using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.RacketCart;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.RacketCart;

namespace TennisAcademyApp.Controllers
{
    public class RacketCartController : BaseController
    {
        private readonly IRacketCartService cartService;

        public RacketCartController(IRacketCartService cartService)
        {
            this.cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                string userId = GetUserId()!;
                var racketsInCart = await this.cartService.GetAllRacketsInCartAsync(userId);
                return View(racketsInCart);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = CannotLoadRacketCartErrorMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRacketToCart(int racketid, int quantity)
        {
            try
            {
                string userId = GetUserId()!;
                bool isAdded = await this.cartService.AddRacketToCartAsync(userId, racketid, quantity);

                if (!isAdded)
                {
                    TempData["ErrorMessage"] = InvalidQuantityErrorMessage;
                    return RedirectToAction(nameof(Index), "Racket");
                }

                TempData["SuccessMessage"] = RacketAddedToCartSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = InvalidQuantityErrorMessage;
                return RedirectToAction(nameof(Index), "Racket");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRacketFromCart(int id)
        {
            try
            {
                string userId = GetUserId()!;
                bool isRemoved = await cartService.RemoveRacketFromCartAsync(userId, id);

                if (!isRemoved)
                {
                    TempData["ErrorMessage"] = RacketFailedToRemoveFromCartErrorMessage;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = RacketRemovedFromCartSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                TempData["ErrorMessage"] = RacketFailedToRemoveFromCartErrorMessage;
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException)
            {
                TempData["ErrorMessage"] = RacketNotFoundInCartErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> RacketCheckout()
        {
            try
            {
                string userId = GetUserId()!;
                bool isCheckedOut = await cartService.CheckOutAllRacketsAsync(userId);

                if (!isCheckedOut)
                {
                    TempData["ErrorMessage"] = UnableToCheckoutErrorMessage;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = RacketCheckoutSuccessful;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = UnableToCheckoutErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
