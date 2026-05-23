using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Coach;
using static TennisAcademyApp.GCommon.Validations.ValidationConstants.Coach;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Coach;

namespace TennisAcademyApp.Services.Core
{
    public class FavouriteCoachService : IFavouriteCoachService
    {
        private readonly TennisAcademyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        public FavouriteCoachService(TennisAcademyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public async Task<IEnumerable<FavouriteCoachViewModel>> GetFavouritesAsync(string? userId)
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture == "bg";

            var favourites = await dbContext.UserFavourites
                .Include(c => c.Coach)
                .AsNoTracking()
                .Where(uc => uc.UserId == userId)
                .Select(uc => new FavouriteCoachViewModel
                {
                    CoachId = uc.CoachId,
                    CoachName = isBg ? uc.Coach.NameBg : uc.Coach.Name,
                    CoachAge = uc.Coach.Age,
                    ImageUrl = uc.Coach.ImageUrl ?? NoImageUrl,
                    Description = isBg ? uc.Coach.DescriptionBg : uc.Coach.Description
                })
                .ToListAsync();

            return favourites;
        }

        public async Task<bool> AddFavouriteCoachAsync(string userId, int id)
        {
            bool result = false;
            var user = await userManager.FindByIdAsync(userId);

            var coach = await dbContext.Coaches.FindAsync(id);
            if (coach == null)
            {
                throw new ArgumentException(CoachNotFoundErrorMessage);
            }
            if (user != null)
            {
                var favouriteCoach = await dbContext
                    .UserFavourites
                    .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CoachId == id);

                if (favouriteCoach == null)
                {
                    favouriteCoach = new UserFavourite()
                    {
                        UserId = userId,
                        CoachId = id,
                    };
                }
                else
                {
                    throw new ArgumentException(CoachAlreadyAddedToFavouritesErrorMessage);
                }
                    await dbContext.UserFavourites.AddAsync(favouriteCoach);
                    await dbContext.SaveChangesAsync();

                    result = true;
                }
            return result;
        }

        public async Task<bool> RemoveFromFavouritesAsync(string userId, int id)
        {
            bool result = false;

            var user = await userManager.FindByIdAsync(userId);

            var coach = await dbContext.Coaches.FindAsync(id);
            if (user != null && coach != null)
            {
                var favouriteCoach = await dbContext
                    .UserFavourites
                    .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CoachId == id);

                if (favouriteCoach != null)
                {
                    dbContext.UserFavourites.Remove(favouriteCoach);
                    await dbContext.SaveChangesAsync();

                    result = true;
                }
            }
            return result;
        }
    }
}
