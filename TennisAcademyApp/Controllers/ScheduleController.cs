using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TennisAcademyApp.Services.Core.Contracts;
using static TennisAcademyApp.GCommon.Validations.ValidationConstants;

namespace TennisAcademyApp.Controllers
{
    [Authorize(Roles = Trainer)]
    public class ScheduleController : Controller
    {
        private readonly ICoachService coachService;

        public ScheduleController(ICoachService coachService)
        {
            this.coachService = coachService;
        }

        public async Task<IActionResult> MySchedule()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var schedule = await coachService.GetTrainerScheduleAsync(userId);

            return View(schedule);
        }
    }
}