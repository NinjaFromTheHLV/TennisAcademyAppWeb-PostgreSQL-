using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Admin.UserManagement;
using TennisAcademyApp.ViewModels.DropDown;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.UserManagement;
using static TennisAcademyApp.GCommon.Validations.ValidationConstants;

namespace TennisAcademyApp.Services.Core
{
    public class UserService : IUserService
    {
        private readonly TennisAcademyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UserService(UserManager<ApplicationUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           TennisAcademyDbContext dbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<UserIndexViewModel>> GetUserManagementDataAsync(string userId)
        {
            var usersList = await userManager.Users
                 .Where(u => u.Id != userId)
                 .ToListAsync();

            var result = new List<UserIndexViewModel>();

            foreach (var u in usersList)
            {
                var roles = await roleManager.Roles
                    .Select(r => r.Name)
                    .ToListAsync();

                result.Add(new UserIndexViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    Roles = await userManager.GetRolesAsync(u),
                    AllExistingRoles = roles
                });
            }

            return result;
        }

        public async Task<bool> AssignUserToRoleAsync(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            bool roleExists = await roleManager.RoleExistsAsync(role);

            if (user == null || roleExists == false)
            {
                return false;
            }

            bool alreadyInRole = await userManager.IsInRoleAsync(user, role);

            if (!alreadyInRole)
            {
                var result = await userManager.AddToRoleAsync(user, role);

                if (!result.Succeeded)
                {
                    return false;
                }
                if (role == Trainer) 
                {
                    bool coachExists = await dbContext.Coaches.AnyAsync(c => c.UserId == userId);

                    if (!coachExists)
                    {
                        var coach = new Data.Models.Coach
                        {
                            Name = $"{user.FirstName} {user.LastName}",
                            NameBg = $"{user.FirstName} {user.LastName}",
                            ImageUrl = "~/pictures/DefaultUserImage.webp",
                            Age = 30,
                            Nationality = "Unknown",
                            NationalityBg = "Неизвестна",
                            Description = "Please update your profile description.",
                            DescriptionBg = "Моля, обновете описанието на профила си.",
                            IsDeleted = false,
                            UserId = userId
                        };

                        await dbContext.Coaches.AddAsync(coach);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            else
            {
                throw new InvalidOperationException(UserAlreadyInRoleErrorMessage);
            }

            return true;
        }
        public async Task<bool> RemoveUserFromRoleAsync(string userId, string role)
        {
            bool isRemoved = false;
            var user = await userManager.FindByIdAsync(userId);

            bool roleExists = await roleManager.RoleExistsAsync(role);

            if (user == null || !roleExists)
            {
                return false;
            }

            bool alreadyInRole = await userManager.IsInRoleAsync(user, role);

            if (alreadyInRole)
            {
                var result = await userManager.RemoveFromRoleAsync(user, role);
            }

            isRemoved = true;
            return isRemoved;
        }

        public async Task<bool> RemoveUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var userReservations = dbContext.Reservations.Where(r => r.PlayerId == userId);
            dbContext.Reservations.RemoveRange(userReservations);

            var userFavourites = dbContext.UserFavourites.Where(uf => uf.UserId == userId);
            dbContext.UserFavourites.RemoveRange(userFavourites);

            dbContext.RacketCart.RemoveRange(dbContext.RacketCart.Where(rc => rc.UserId == userId));
            dbContext.BallCart.RemoveRange(dbContext.BallCart.Where(bc => bc.UserId == userId));
            dbContext.BagCart.RemoveRange(dbContext.BagCart.Where(bc => bc.UserId == userId));

            var tournamentRegistrations = dbContext.TournamentsUsers.Where(tu => tu.UserId == userId);
            dbContext.TournamentsUsers.RemoveRange(tournamentRegistrations);

            var coach = await dbContext.Coaches.FirstOrDefaultAsync(c => c.UserId == userId);
            if (coach != null)
            {
                dbContext.Coaches.Remove(coach);
            }

            await dbContext.SaveChangesAsync();

            var result = await userManager.DeleteAsync(user);
            return result.Succeeded;

        }
    }
}
