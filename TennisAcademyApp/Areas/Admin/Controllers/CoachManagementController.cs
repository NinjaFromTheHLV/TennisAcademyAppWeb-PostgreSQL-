using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Coach;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Coach;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.Coach;

namespace TennisAcademyApp.Areas.Admin.Controllers
{
    public class CoachManagementController : AdminBaseController
    {
        private readonly ICoachService coachService;
        public CoachManagementController(ICoachService coachService)
        {
            this.coachService = coachService;
        }

        public async Task<IActionResult> Index(string? searchQuery = null, int page = 1, int pageSize = 3)
        {
            try
            {
                var model = await coachService.GetCoachesByPageAsync(searchQuery, page, pageSize);

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                await Task.CompletedTask;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCoachInputModel inputModel)
        {
            try
            {
                string userId = GetUserId()!;
                if (ModelState.IsValid == false)
                {
                    TempData["ErrorMessage"] = InvalidData;
                    return View(inputModel);
                }
                await this.coachService.AddCoachAsync(userId, inputModel);
                TempData["SuccessMessage"] = CoachAddedSuccessfully;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                string user = GetUserId()!;
                var coach = await this.coachService.GetCoachForEdittingAsync(user, id);

                return View(coach);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CoachEditInputModel model)
        {
            string userId = GetUserId()!;

            if (ModelState.IsValid == false)
            {
                TempData["ErrorMessage"] = InvalidData;
                return View(model);
            }

            try
            {
                await this.coachService.EdittedCoachAsync(userId, model);
                TempData["SuccessMessage"] = CoachUpdatedSuccessfully;

                return RedirectToAction(nameof(Index));
            }

            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError(string.Empty, "Внимание: Записът беше променен от друг потребител, докато го редактирахте. Вашите промени не бяха запазени. Моля, заредете страницата наново и опитайте отново.");
                TempData["ErrorMessage"] = "Грешка при дублиран достъп (Concurrency Error).";

                return View(model);
            }
            catch (ArgumentException)
            {
                TempData["ErrorMessage"] = CoachEditErrorMessage;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Възникна неочаквана грешка при запис на данните.";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string userId = GetUserId()!;
                var delete = await this.coachService.GetCoachForDeletingAsync(userId, id);

                if (delete == null)
                {
                    TempData["ErrorMessage"] = CoachNotFoundErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
                return View(delete);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCoachViewModel model)
        {
            try
            {
                string userId = GetUserId()!;

                bool result = await this.coachService.DeletedCoachAsync(userId, model);

                if (result == false)
                {
                    TempData["ErrorMessage"] = CoachDeleteErrorMessage;
                    return RedirectToAction(nameof(Delete), new { id = model.CoachId });
                }
                TempData["SuccessMessage"] = CoachDeletedSuccessfully;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = CoachDeleteErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}