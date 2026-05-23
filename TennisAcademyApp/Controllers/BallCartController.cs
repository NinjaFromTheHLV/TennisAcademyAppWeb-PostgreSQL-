using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.BallCart;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.BallCart;

namespace TennisAcademyApp.Controllers
{
    public class BallCartController : BaseController
    {
        private readonly IBallCartService cartService;

        public BallCartController(IBallCartService cartService)
        {
            this.cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                string userId = GetUserId()!;
                var ballsInCart = await this.cartService.GetAllBallsInCartAsync(userId);
                return View(ballsInCart);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = CannotLoadBallCartErrorMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBallToCart(int ballId, int quantity)
        {
            try
            {
                string userId = GetUserId()!;
                bool isAdded = await this.cartService.AddBallToCartAsync(userId, ballId, quantity);

                if (!isAdded)
                {
                    TempData["ErrorMessage"] = InvalidQuantityErrorMessage;
                    return RedirectToAction(nameof(Index), "Ball");
                }

                TempData["SuccessMessage"] = BallAddedToCartSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = InvalidQuantityErrorMessage;
                return RedirectToAction(nameof(Index), "Ball");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveBallFromCart(int id)
        {
            try
            {
                string userId = GetUserId()!;
                bool isRemoved = await cartService.RemoveBallFromCartAsync(userId, id);

                if (!isRemoved)
                {
                    TempData["ErrorMessage"] = BallFailedToRemoveFromCartErrorMessage;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = BallRemovedFromCartSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                TempData["ErrorMessage"] = BallFailedToRemoveFromCartErrorMessage;
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException)
            {
                TempData["ErrorMessage"] = BallNotFoundInCartErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> BallCheckout()
        {
            try
            {
                string userId = GetUserId()!;
                bool isCheckedOut = await cartService.CheckOutAllBallsAsync(userId);

                if (!isCheckedOut)
                {
                    TempData["ErrorMessage"] = UnableToCheckoutErrorMessage;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = BallCheckoutSuccessful;
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
