using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;

namespace TennisAcademyApp.Controllers
{
    [Authorize]
    public class RankingController : Controller
    {
        private readonly IRankingService rankingService;

        public RankingController(IRankingService rankingService)
        {
            this.rankingService = rankingService;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var leaderboard = await rankingService.GetLeaderboardAsync();
            return View(leaderboard);
        }
    }
}