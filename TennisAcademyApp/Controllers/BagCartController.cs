using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.BagCart;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.BagCart;

namespace TennisAcademyApp.Controllers
{
    public class BagCartController : BaseController
    {
        private readonly IBagCartService cartService;

        public BagCartController(IBagCartService cartService)
        {
            this.cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                string userId = GetUserId()!;
                var bagsInCart = await this.cartService.GetAllBagsInCartAsync(userId);
                return View(bagsInCart);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = CannotLoadBagCartErrorMessage;
                return RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBagToCart(int id, int quantity)
        {
            try
            {
                string userId = GetUserId()!;
                bool isAdded = await this.cartService.AddBagToCartAsync(userId, id, quantity);

                if (!isAdded)
                {
                    TempData["ErrorMessage"] = InvalidQuantityErrorMessage;
                    return RedirectToAction(nameof(Index), "Bag");
                }

                TempData["SuccessMessage"] = BagAddedToCartSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = InvalidQuantityErrorMessage;
                return RedirectToAction(nameof(Index), "Bag");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveBagFromCart(int id)
        {
            try
            {
                string userId = GetUserId()!;
                bool isRemoved = await cartService.RemoveBagFromCartAsync(userId, id);

                if (!isRemoved)
                {
                    TempData["ErrorMessage"] = BagFailedToRemoveFromCartErrorMessage;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = BagRemovedFromCartSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = BagFailedToRemoveFromCartErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> BagCheckout()
        {
            try
            {
                string userId = GetUserId()!;
                bool isCheckedOut = await cartService.CheckOutAllBagsAsync(userId);

                if (!isCheckedOut)
                {
                    TempData["ErrorMessage"] = UnableToCheckoutErrorMessage;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = BagCheckoutSuccessful;
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
