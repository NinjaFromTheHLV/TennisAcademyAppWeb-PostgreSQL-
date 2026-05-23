using TennisAcademyApp.Data.Models;
using TennisAcademyApp.ViewModels.Ball;

namespace TennisAcademyApp.Services.Core.Contracts
{
    public interface IBallService
    {
        Task<IEnumerable<BallIndexViewModel>> GetAllBallsAsync();
        Task<Ball> FindBallByIdAsync(int? id);
        Task<bool> AddBallAsync(string userId, BallCreateInputModel model);
        Task<BallEditFormModel> GetBallForEditingAsync(string userId, int? id);
        Task<bool> EditBallAsync(BallEditFormModel model);
        Task<BallDeleteViewModel> GetBallForDeletingAsync(string userId, int? id);
        Task<bool> DeleteBallAsync(string userId, BallDeleteViewModel model);
    }
}
