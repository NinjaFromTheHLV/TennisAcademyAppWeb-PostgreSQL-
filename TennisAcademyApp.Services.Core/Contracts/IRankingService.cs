using TennisAcademyApp.ViewModels.Ranking;

namespace TennisAcademyApp.Services.Core.Contracts
{
    public interface IRankingService
    {
        Task<IEnumerable<UserRankingViewModel>> GetLeaderboardAsync();
    }
}