using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.Data;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Ranking;

namespace TennisAcademyApp.Services.Core
{
    public class RankingService : IRankingService
    {
        private readonly TennisAcademyDbContext dbContext;

        public RankingService(TennisAcademyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<UserRankingViewModel>> GetLeaderboardAsync()
        {
            var excludedRoleIds = await dbContext.Roles
                .Where(r => r.Name == "Admin" || r.Name == "Coach")
                .Select(r => r.Id)
                .ToListAsync();

            var usersData = await dbContext.Users
                .Where(u => !dbContext.UserRoles.Any(ur => ur.UserId == u.Id && excludedRoleIds.Contains(ur.RoleId)))
                .Select(u => new UserRankingViewModel
                {
                    FullName = $"{u.FirstName} {u.LastName}",
                    TournamentsCount = dbContext.TournamentsUsers.Count(tu => tu.UserId == u.Id),
                    ReservationsCount = dbContext.Reservations.Count(r => r.PlayerId == u.Id && r.IsCompleted),

                    BoughtItemsCount =
                        (dbContext.RacketCart.Where(c => c.UserId == u.Id && c.IsOrdered).Sum(c => (int?)c.Quantity) ?? 0) +
                        (dbContext.BallCart.Where(c => c.UserId == u.Id && c.IsOrdered).Sum(c => (int?)c.Quantity) ?? 0) +
                        (dbContext.BagCart.Where(c => c.UserId == u.Id && c.IsOrdered).Sum(c => (int?)c.Quantity) ?? 0)
                })
                .ToListAsync();

            var sortedLeaderboard = usersData
                .OrderByDescending(u => u.TotalPoints)
                .ToList();

            for (int i = 0; i < sortedLeaderboard.Count; i++)
            {
                sortedLeaderboard[i].Position = i + 1;
            }

            return sortedLeaderboard;
        }
    }
}