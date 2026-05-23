using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Tournament;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.Tournament;

namespace TennisAcademyApp.Controllers
{
    [Authorize]
    public class TournamentController : Controller
    {
        private readonly ITournamentService tournamentService;

        public TournamentController(ITournamentService tournamentService)
        {
            this.tournamentService = tournamentService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All(int? categoryId, string? searchTerm, int page = 1)
        {
            var queryModel = await tournamentService.GetAllTournamentsAsync(categoryId, searchTerm, page, 6);
            return View(queryModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var tournament = await tournamentService.GetTournamentDetailsAsync(id, userId);

            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var success = await tournamentService.EnrollUserAsync(id, userId);

            if (!success)
            {
                TempData["ErrorMessage"] = TournamentEnrollError;
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["SuccessMessage"] = TournamentEnrollSuccess;
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Unenroll(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var success = await tournamentService.UnenrollUserAsync(id, userId);

            if (!success)
            {
                TempData["ErrorMessage"] = TournamentUnenrollError;
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["SuccessMessage"] = TournamentUnenrollSuccess;
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}