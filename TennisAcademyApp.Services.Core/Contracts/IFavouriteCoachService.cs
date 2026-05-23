using TennisAcademyApp.ViewModels.Coach;

namespace TennisAcademyApp.Services.Core.Contracts
{
    public interface IFavouriteCoachService
    {
        Task<IEnumerable<FavouriteCoachViewModel>> GetFavouritesAsync(string? userId);
        Task<bool> AddFavouriteCoachAsync(string userId, int id);
        Task<bool> RemoveFromFavouritesAsync(string userId, int id);
    }
}
