using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Contracts;
using TennisAcademyApp.Services.Core.Contracts;

namespace TennisAcademyApp.Controllers
{
    [Authorize]
    public class WheelController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWheelService _wheelService;
        private readonly TennisAcademyDbContext _context;

        public WheelController(
            UserManager<ApplicationUser> userManager,
            IWheelService wheelService,
            TennisAcademyDbContext context)
        {
            _userManager = userManager;
            _wheelService = wheelService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var items = await _wheelService.GetWheelItemsAsync();

            var racket = await _context.Rackets.FirstOrDefaultAsync();
            var ball = await _context.Balls.FirstOrDefaultAsync();
            var bag = await _context.Bags.FirstOrDefaultAsync();

            ViewBag.RacketName = items.RacketName;
            ViewBag.RacketImg = racket?.ImageUrl;
            ViewBag.BallName = items.BallName;
            ViewBag.BallImg = ball?.ImageUrl;
            ViewBag.BagName = items.BagName;
            ViewBag.BagImg = bag?.ImageUrl;

            var eligibility = await _wheelService.GetUserEligibilityAsync(user.Id);

            ViewBag.CanSpin = eligibility.CanSpin;
            ViewBag.NextSpinDate = eligibility.NextSpinDate?.ToString("dd.MM.yyyy HH:mm");

            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Spin()
        {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                var result = await _wheelService.SpinWheelAsync(user.Id);

                if (!result.Success)
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Json(new { winningIndex = result.WinningIndex, prize = result.Prize });
        }
    }
}