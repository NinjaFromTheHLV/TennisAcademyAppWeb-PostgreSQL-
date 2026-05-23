using TennisAcademyApp.Data.Models;
using TennisAcademyApp.ViewModels.Bag;

namespace TennisAcademyApp.Services.Core.Contracts
{
    public interface IBagService
    {
        Task<IEnumerable<BagIndexViewModel>> GetAllBagsAsync();
        Task<Bag> FindBagByIdAsync(int? id);
        Task<bool> AddBagAsync(string userId, BagCreateInputModel model);
        Task<BagEditFormModel> GetBagForEditingAsync(string userId, int? id);
        Task<bool> EditBagAsync(BagEditFormModel model);
        Task<BagDeleteViewModel> GetBagForDeletingAsync(string userId, int? id);
        Task<bool> DeleteBagAsync(string userId, BagDeleteViewModel model);
    }
}
