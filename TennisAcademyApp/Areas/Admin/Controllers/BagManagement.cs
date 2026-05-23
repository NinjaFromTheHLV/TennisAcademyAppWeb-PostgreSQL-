using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Bag;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.Bag;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Bag;

namespace TennisAcademyApp.Areas.Admin.Controllers
{
    public class BagManagement : AdminBaseController
    {
        private readonly IBagService bagService;
        public BagManagement(IBagService bagService)
        {
            this.bagService = bagService;
        }
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
        public async Task<IActionResult> Create(BagCreateInputModel inputModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = InvalidData;
                    return View(inputModel);
                }

                string userId = GetUserId()!;
                bool isAdded = await this.bagService.AddBagAsync(userId, inputModel);

                if (!isAdded)
                {
                    TempData["ErrorMessage"] = BagAddErrorMessage;
                    return View(inputModel);
                }

                TempData["SuccessMessage"] = BagAddedSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = BagAddErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                string userId = GetUserId()!;
                var bag = await this.bagService.GetBagForEditingAsync(userId, id);

                return View(bag);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BagEditFormModel editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = InvalidData;
                    return RedirectToAction(nameof(Edit), new { id = editModel.Id });
                }

                bool isEdited = await this.bagService.EditBagAsync(editModel);

                if (!isEdited)
                {
                    TempData["ErrorMessage"] = BagEditErrorMessage;
                    return RedirectToAction(nameof(Edit), new { id = editModel.Id });
                }

                TempData["SuccessMessage"] = BagUpdatedSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = BagEditErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string userId = GetUserId()!;
                var bag = await this.bagService.GetBagForDeletingAsync(userId, id);

                return View(bag);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BagDeleteViewModel deleteModel)
        {
            try
            {
                bool isDeleted = await this.bagService.DeleteBagAsync(GetUserId()!, deleteModel);

                if (!isDeleted)
                {
                    TempData["ErrorMessage"] = BagDeleteErrorMessage;
                    return RedirectToAction(nameof(Delete), new { id = deleteModel.Id });
                }

                TempData["SuccessMessage"] = BagDeletedSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = BagDeleteErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
