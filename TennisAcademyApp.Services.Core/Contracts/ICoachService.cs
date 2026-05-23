using TennisAcademyApp.Data.Models;
using TennisAcademyApp.ViewModels.Coach;
using TennisAcademyApp.ViewModels.DropDown;

namespace TennisAcademyApp.Services.Core.Contracts
{
    public interface ICoachService
    {
        Task<PaginatedCoachesViewModel> GetCoachesByPageAsync(string? searchQuery, int page, int pageSize);
        Task<IEnumerable<CoachDropDownModel>> GetGoachesForDropDownAsync();
        Task<Coach?> GetCoachByIdAsync(int? id);
        Task <CoachDetailsViewModel> GetCoachDetailsAsync(string userId, int id);
        Task<bool> AddCoachAsync(string userId, AddCoachInputModel inputModel);
        Task<CoachEditInputModel> GetCoachForEdittingAsync(string userId, int id);
        Task<bool> EdittedCoachAsync(string userId, CoachEditInputModel model);
        Task<DeleteCoachViewModel?> GetCoachForDeletingAsync(string userId,int id);
        Task<bool> DeletedCoachAsync(string userId, DeleteCoachViewModel model);
        Task<IEnumerable<CoachScheduleViewModel>> GetTrainerScheduleAsync(string userId);
    }
}
