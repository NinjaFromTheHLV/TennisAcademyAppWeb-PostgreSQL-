using TennisAcademyApp.ViewModels.DropDown;

namespace TennisAcademyApp.Services.Core.Contracts
{
    public interface ISurfaceService
    {
        Task<IEnumerable<SurfaceDropDownModel>> GetSurfacesForDropDownAsync();
    }
}
