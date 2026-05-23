using TennisAcademyApp.ViewModels.Cart;
namespace TennisAcademyApp.Services.Core.Contracts
{
    public interface IBallCartService
    {
        Task<IEnumerable<BallCartIndexViewModel>> GetAllBallsInCartAsync(string userId);
        Task<bool> AddBallToCartAsync(string userId, int ballId, int quantity);
        Task<bool> RemoveBallFromCartAsync(string userId, int ballId);
        Task<bool> CheckOutAllBallsAsync(string userId);
    }
}
