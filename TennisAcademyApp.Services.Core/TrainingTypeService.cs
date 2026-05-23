using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.Data;
using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.ViewModels.DropDown;

namespace TennisAcademyApp.Services.Core
{
    public class TrainingTypeService : ITrainingTypeService
    {
        private readonly TennisAcademyDbContext dbContext;
        public TrainingTypeService(TennisAcademyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<TrainingTypeDropDownModel>> GetAllTrainingTypesForDropDownAsync()
        {
            IEnumerable<TrainingTypeDropDownModel> trainingTypes = await dbContext.Trainings
                .AsNoTracking()
                .Select(tt => new TrainingTypeDropDownModel
                {
                    Id = tt.Id,
                    Name = tt.Name,
                    NameBg = tt.NameBg
                })
                .ToListAsync();

            return trainingTypes;
        }
    }
}
