using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.Data;
using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.ViewModels.DropDown;

namespace TennisAcademyApp.Services.Core
{
    public class SurfaceService : ISurfaceService
    {
        private readonly TennisAcademyDbContext dbContext;
        public SurfaceService(TennisAcademyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<SurfaceDropDownModel>> GetSurfacesForDropDownAsync()
        {
            IEnumerable<SurfaceDropDownModel> surfaces = await dbContext.Surfaces
                .Select(s => new SurfaceDropDownModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    NameBg = s.NameBg
                })
                .ToListAsync();

            return surfaces;
        }
    }
}
