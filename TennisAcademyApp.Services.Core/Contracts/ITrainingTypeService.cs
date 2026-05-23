using TennisAcademyApp.ViewModels.DropDown;

namespace TennisAcademyApp.Services.Core.Contracts
{
    public interface ITrainingTypeService
    {
        Task<IEnumerable<TrainingTypeDropDownModel>> GetAllTrainingTypesForDropDownAsync();
    }
}
