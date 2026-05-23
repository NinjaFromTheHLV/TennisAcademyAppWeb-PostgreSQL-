using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.DropDown;
using TennisAcademyApp.ViewModels.Reservation;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Reservation;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.Reservation;

namespace TennisAcademyApp.Controllers
{
    [Authorize(Roles = "User")]
    public class ReservationController : BaseController
    {
        private readonly IReservationService reservationService;
        private readonly ISurfaceService surfaceService;
        private readonly ITrainingTypeService trainingTypeService;
        private readonly ICoachService coachService;

        public ReservationController(IReservationService reservationService,
                                    ISurfaceService surfaceService,
                                    ITrainingTypeService trainingTypeService,
                                    ICoachService coachService)
        {
            this.reservationService = reservationService;
            this.surfaceService = surfaceService;
            this.trainingTypeService = trainingTypeService;
            this.coachService = coachService;
        }

        [HttpGet]
        public async Task<IActionResult> ExportToJson(string? searchTerm, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var dataToExport = await this.reservationService.GetAllReservationsForExportAsync(searchTerm, fromDate, toDate);

                var jsonOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
                string jsonString = System.Text.Json.JsonSerializer.Serialize(dataToExport, jsonOptions);

                byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);
                string fileName = $"Reservations_Export_{DateTime.Now:yyyyMMdd_HHmmss}.json";

                return File(fileBytes, "application/json", fileName);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Грешка при генериране на JSON експортния файл.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, DateTime? fromDate, DateTime? toDate, string? sortOrder)
        {
            try
            {
                string? userId = GetUserId();

                ViewData["CurrentSearch"] = searchTerm;
                ViewData["FromDate"] = fromDate?.ToString("yyyy-MM-dd");
                ViewData["ToDate"] = toDate?.ToString("yyyy-MM-dd");

                ViewData["CurrentSort"] = sortOrder;
                ViewData["DateSortParam"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
                ViewData["DurationSortParam"] = sortOrder == "Duration" ? "duration_desc" : "Duration";

                if (userId == null)
                {
                    return RedirectToAction(nameof(Index), "Home");
                }

                var model = await this.reservationService.GetFilteredUserReservationsAsync(userId, searchTerm, fromDate, toDate, sortOrder);

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction(nameof(Index), "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new ReservationCreateInputModel
            {
                Date = DateTime.Now,
                Coaches = new List<CoachDropDownModel>(),
                Surfaces = new List<SurfaceDropDownModel>(),
                TrainingTypes = new List<TrainingTypeDropDownModel>()
            };

            try
            {
                // 2. Пълним списъците един по един. Ако някой гръмне, ще разберем точно кой е в конзолата
                var coachesData = await coachService.GetGoachesForDropDownAsync();
                if (coachesData != null) model.Coaches = coachesData;

                var surfacesData = await surfaceService.GetSurfacesForDropDownAsync();
                if (surfacesData != null) model.Surfaces = surfacesData;

                var trainingTypesData = await trainingTypeService.GetAllTrainingTypesForDropDownAsync();
                if (trainingTypesData != null) model.TrainingTypes = trainingTypesData;

                return View(model);
            }
            catch (Exception ex)
            {
                // Отпечатва точната грешка от Service слоя в Output прозореца на Visual Studio
                Console.WriteLine($"[CREATE RESERVATION ERROR]: {ex.Message}");

                // Връщаме празен модел към View-то, за да НЕ гърми "line 27", а просто падащите менюта да останат празни
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReservationCreateInputModel model)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    TempData["ErrorMessage"] = InvalidData;
                    model.Coaches = await coachService.GetGoachesForDropDownAsync();
                    model.Surfaces = await surfaceService.GetSurfacesForDropDownAsync();
                    model.TrainingTypes = await trainingTypeService.GetAllTrainingTypesForDropDownAsync();

                    return View(model);
                }

                string userId = GetUserId()!;

                await reservationService.CreateReservationAsync(userId, model);
                TempData["SuccessMessage"] = ReservationCreatedSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Coaches = await coachService.GetGoachesForDropDownAsync();
                model.Surfaces = await surfaceService.GetSurfacesForDropDownAsync();
                model.TrainingTypes = await trainingTypeService.GetAllTrainingTypesForDropDownAsync();
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                string userId = GetUserId()!;

                var reservationDetails = await reservationService.GetUserReservationDetailsAsync(userId, id);

                return View(reservationDetails);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                string userId = GetUserId()!;

                var reservationToDelete = await reservationService.GetUserReservationForDeletingAsync(userId, id);

                return View(reservationToDelete);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ReservationDeleteViewModel model)
        {
            try
            {
                string userId = GetUserId()!;

                await reservationService.DeleteReservationAsync(userId, model);
                TempData["SuccessMessage"] = ReservationDeletedSuccessfully;
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                TempData["ErrorMessage"] = ReservationDeleteErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> ReservationHistory()
        {
            try
            {
                string userId = GetUserId()!;

                var reservationHistory = await reservationService.GetUserReservationHistoryAsync(userId);
                return View(reservationHistory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
