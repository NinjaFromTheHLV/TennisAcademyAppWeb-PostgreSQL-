using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Racket;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Racket;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.Racket;

namespace TennisAcademyApp.Areas.Admin.Controllers
{
    public class RacketManagementController : AdminBaseController
    {
        private readonly IRacketService racketService;
        public RacketManagementController(IRacketService racketService)
        {
            this.racketService = racketService;
        }
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
        public async Task<IActionResult> Create(RacketCreateInputModel inputModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = InvalidData;
                    return View(inputModel);
                }

                string userId = GetUserId()!;
                bool isAdded = await this.racketService.AddRacketAsync(userId, inputModel);

                if (!isAdded)
                {
                    TempData["ErrorMessage"] = RacketAddErrorMessage;
                    return View(inputModel);
                }

                TempData["SuccessMessage"] = RacketAddedSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = RacketAddErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                string userId = GetUserId()!;
                var racket = await this.racketService.GetRacketForEdittingAsync(userId, id);

                return View(racket);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RacketEditFormModel editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = InvalidData;
                    return RedirectToAction(nameof(Edit), new { id = editModel.Id });
                }

                bool isEdited = await this.racketService.EditRacketAsync(editModel);

                if (!isEdited)
                {
                    TempData["ErrorMessage"] = RacketEditErrorMessage;
                    return RedirectToAction(nameof(Edit), new { id = editModel.Id });
                }

                TempData["SuccessMessage"] = RacketUpdatedSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = RacketEditErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string userId, int id)
        {
            try
            {
                userId = GetUserId()!;
                var racket = await this.racketService.GetRacketForDeletingAsync(userId, id);

                return View(racket);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool isDeleted = await this.racketService.DeleteRacketAsync(GetUserId()!, id);

                if (!isDeleted)
                {
                    TempData["ErrorMessage"] = RacketDeleteErrorMessage;
                    return RedirectToAction(nameof(Delete), new { id = id });
                }

                TempData["SuccessMessage"] = RacketDeletedSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = RacketDeleteErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
