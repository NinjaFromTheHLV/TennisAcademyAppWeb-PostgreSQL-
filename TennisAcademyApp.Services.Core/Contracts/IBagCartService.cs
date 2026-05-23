using TennisAcademyApp.ViewModels.Cart;

namespace TennisAcademyApp.Services.Core.Contracts
{
    public interface IBagCartService
    {
        Task<IEnumerable<BagCartIndexViewModel>> GetAllBagsInCartAsync(string userId);
        Task<bool> AddBagToCartAsync(string userId, int bagId, int quantity);
        Task<bool> RemoveBagFromCartAsync(string userId, int bagId);
        Task<bool> CheckOutAllBagsAsync(string userId);
    }
}
