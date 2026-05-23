using TennisAcademyApp.ViewModels.Cart;

namespace TennisAcademyApp.Services.Core.Contracts
{
    public interface IRacketCartService
    {
        Task<IEnumerable<RacketCartIndexViewModel>> GetAllRacketsInCartAsync(string userId); 
        Task<bool> AddRacketToCartAsync(string userId, int racketId, int quantity);
        Task<bool> RemoveRacketFromCartAsync(string userId, int racketId);
        Task<bool> CheckOutAllRacketsAsync(string userId);
    }
}
