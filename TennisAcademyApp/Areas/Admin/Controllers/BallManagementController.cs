using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Ball;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Ball;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.Ball;

namespace TennisAcademyApp.Areas.Admin.Controllers
{
    public class BallManagementController : AdminBaseController
    {
        private readonly IBallService ballService;
        public BallManagementController(IBallService ballService)
        {
            this.ballService = ballService;
        }
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                await Task.CompletedTask;
                return View();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = UnexpectedError;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(BallCreateInputModel inputModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = InvalidData;
                    return View(inputModel);
                }

                string userId = GetUserId()!;
                bool isAdded = await this.ballService.AddBallAsync(userId, inputModel);

                if (!isAdded)
                {
                    TempData["ErrorMessage"] = BallAddErrorMessage;
                    return View(inputModel);
                }

                TempData["SuccessMessage"] = BallAddedSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = BallAddErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                string userId = GetUserId()!;
                var ball = await this.ballService.GetBallForEditingAsync(userId, id);

                return View(ball);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BallEditFormModel editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = InvalidData;
                    return RedirectToAction(nameof(Edit), new { id = editModel.Id });
                }

                bool isEdited = await this.ballService.EditBallAsync(editModel);

                if (!isEdited)
                {
                    TempData["ErrorMessage"] = BallEditErrorMessage;
                    return RedirectToAction(nameof(Edit), new { id = editModel.Id });
                }

                TempData["SuccessMessage"] = BallUpdatedSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = BallEditErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string userId = GetUserId()!;
                var ball = await this.ballService.GetBallForDeletingAsync(userId, id);

                return View(ball);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BallDeleteViewModel deleteModel)
        {
            try
            {
                bool isDeleted = await this.ballService.DeleteBallAsync(GetUserId()!, deleteModel);

                if (!isDeleted)
                {
                    TempData["ErrorMessage"] = BallDeleteErrorMessage;
                    return RedirectToAction(nameof(Delete), new { id = deleteModel.Id });
                }

                TempData["SuccessMessage"] = BallDeletedSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = BallDeleteErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
