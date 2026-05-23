using DeepL;
using GTranslate.Translators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Coach;
using TennisAcademyApp.ViewModels.DropDown;
using TennisAcademyApp.ViewModels.Reservation;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Coach;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.User;
using static TennisAcademyApp.GCommon.Validations.ValidationConstants;
using static TennisAcademyApp.GCommon.Validations.ValidationConstants.Coach;
using static TennisAcademyApp.GCommon.TextUtility.TextUtility;

namespace TennisAcademyApp.Services.Core
{
    public class CoachService : ICoachService
    {
        private readonly TennisAcademyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Translator translator;
        private readonly IConfiguration configuration;

        public CoachService(TennisAcademyDbContext dbContext, UserManager<ApplicationUser> userManager, IConfiguration configuration, Translator translator)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.configuration = configuration;
            this.translator = translator;
        }

        public async Task<IEnumerable<CoachScheduleViewModel>> GetTrainerScheduleAsync(string userId)
        {
            return await dbContext.Reservations
                .Where(r => r.Coach.UserId == userId && !r.IsDeleted)
                .OrderBy(r => r.Date)
                .Select(r => new CoachScheduleViewModel
                {
                    ReservationId = r.Id,
                    Date = r.Date,
                    Duration = r.Duration,
                    Note = r.Note,
                    NoteBg = r.NoteBg,
                    SurfaceName = r.Surface.Name,
                    SurfaceNameBg = r.Surface.NameBg,
                    TrainingTypeName = r.TrainingType.Name,
                    TrainingTypeNameBg = r.TrainingType.NameBg,
                    PlayerName = r.Player.UserName ?? "",
                    PlayerEmail = r.Player.Email ?? ""
                })
                .ToListAsync();
        }

        public async Task<PaginatedCoachesViewModel> GetCoachesByPageAsync(string? searchQuery, int page, int pageSize)
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture.Equals("bg", StringComparison.OrdinalIgnoreCase);

            var query = dbContext.Coaches.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(c => c.Name.Contains(searchQuery) || c.NameBg.Contains(searchQuery));
            }

            var totalCoaches = await query.CountAsync();

            var coaches = await query
                .OrderBy(c => c.CoachId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new AllCoachesViewModel
                {
                    CoachId = c.CoachId,
                    CoachName = isBg ? c.NameBg : c.Name,
                    CoachAge = c.Age,
                    ImageUrl = c.ImageUrl
                })
                .ToListAsync();

            foreach (var coach in coaches)
            {
                if (coach.ImageUrl.IsNullOrEmpty())
                {
                    coach.ImageUrl = NoImageUrl;
                }
            }

            var totalPages = (int)Math.Ceiling(totalCoaches / (double)pageSize);

            return new PaginatedCoachesViewModel
            {
                Coaches = coaches,
                PageNumber = page,
                TotalPages = totalPages,
                SearchQuery = searchQuery
            };
        }

        public async Task<IEnumerable<CoachDropDownModel>> GetGoachesForDropDownAsync()
        {
            return await dbContext.Coaches
                .AsNoTracking()
                .Select(c => new CoachDropDownModel
                {
                    Id = c.CoachId,
                    Name = c.Name,
                    NameBg = c.NameBg,
                    ImageUrl = c.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<CoachDetailsViewModel> GetCoachDetailsAsync(string userId, int id)
        {
            var coach = await dbContext.Coaches.FirstOrDefaultAsync(c => c.CoachId == id);

            if (coach == null)
            {
                throw new ArgumentException(CoachNotFoundErrorMessage);
            }

            var currentCulture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture.Equals("bg", StringComparison.OrdinalIgnoreCase);

            var coachReservations = await dbContext.Reservations
                .Where(r => r.CoachId == id && r.IsDeleted == false)
                .OrderBy(r => r.Date)
                .Select(r => new ReservationIndexViewModel
                {
                    ReservationId = r.Id,
                    Date = r.Date.ToString("dd.MM.yyyy HH:mm"),
                    TrainingTypeName = isBg ? r.TrainingType.NameBg : r.TrainingType.Name
                })
                .ToListAsync();

            return new CoachDetailsViewModel
            {
                CoachId = coach.CoachId,
                CoachAge = coach.Age,
                ImageUrl = coach.ImageUrl,
                CoachReservations = coachReservations,
                CoachName = isBg ? coach.NameBg : coach.Name,
                Description = isBg ? coach.DescriptionBg : coach.Description,
                Nationality = isBg ? coach.NationalityBg : coach.Nationality,
                IsInUserFavorites = userId != null && await dbContext.UserFavourites.AnyAsync(uc => uc.UserId == userId && uc.CoachId == coach.CoachId)
            };
        }

        public async Task<bool> AddCoachAsync(string userId, AddCoachInputModel inputModel)
        {
            var adminUser = await userManager.FindByIdAsync(userId);
            if (adminUser == null || !await userManager.IsInRoleAsync(adminUser, "Admin"))
                throw new ArgumentException("You must be an admin to add a coach.");

            string coachEmail = $"{inputModel.Name.Replace(" ", ".").ToLower()}@tennis.com";
            if (await userManager.FindByEmailAsync(coachEmail) != null)
                throw new ArgumentException("A user with this coach email already exists.");

            var coachUser = new ApplicationUser { Email = coachEmail, UserName = coachEmail, EmailConfirmed = true };
            var createResult = await userManager.CreateAsync(coachUser, configuration["CoachSettings:DefaultPassword"]);
            if (!createResult.Succeeded) throw new Exception("Failed to create Identity User.");

            await userManager.AddToRoleAsync(coachUser, Trainer);

            string nameBg = CapitalizeNames(TransliterateToBg(inputModel.Name));
            string descBg = TransliterateToBg(inputModel.Description);
            string natBg = Capitalize(TransliterateToBg(inputModel.Nationality));

            try
            {
                var trName = await translator.TranslateTextAsync(inputModel.Name, LanguageCode.English, LanguageCode.Bulgarian);
                if (!string.IsNullOrWhiteSpace(trName?.Text))
                {
                    nameBg = string.Join(" ", trName.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                             .Select(word => Capitalize(word)));
                }

                var trDesc = await translator.TranslateTextAsync(inputModel.Description, LanguageCode.English, LanguageCode.Bulgarian);
                if (!string.IsNullOrWhiteSpace(trDesc?.Text)) descBg = trDesc.Text;

                var trNat = await translator.TranslateTextAsync($"He is {inputModel.Nationality}", LanguageCode.English, LanguageCode.Bulgarian);
                if (!string.IsNullOrWhiteSpace(trNat?.Text))
                {
                    string rawNat = trNat.Text.Split(' ').Last().Replace(".", "");
                    natBg = Capitalize(rawNat);
                }
            }
            catch { }

            var coach = new TennisAcademyApp.Data.Models.Coach
            {
                Name = inputModel.Name,
                NameBg = nameBg,
                ImageUrl = inputModel.ImageUrl ?? "~/pictures/DefaultUserImage.webp",
                Age = inputModel.Age,
                Nationality = inputModel.Nationality,
                NationalityBg = natBg,
                Description = inputModel.Description,
                DescriptionBg = descBg,
                UserId = coachUser.Id
            };

            await dbContext.Coaches.AddAsync(coach);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CoachEditInputModel> GetCoachForEdittingAsync(string userId, int id)
        {
            var user = await userManager.FindByIdAsync(userId);
            bool isAdmin = await userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin || userId == null)
            {
                throw new ArgumentException("You must be an admin to edit a coach.");
            }

            if (user == null)
            {
                throw new ArgumentException(UserCannotBeNull);
            }
            var coach = await GetCoachByIdAsync(id);

            return new CoachEditInputModel
            {
                CoachId = coach.CoachId,
                Name = coach.Name,
                Age = coach.Age,
                Nationality = coach.Nationality,
                Description = coach.Description,
                ImageUrl = coach.ImageUrl,
            };
        }

        public async Task<bool> EdittedCoachAsync(string userId, CoachEditInputModel model)
        {
            var coach = await dbContext.Coaches.FirstOrDefaultAsync(c => c.CoachId == model.CoachId);
            if (coach == null)
                throw new ArgumentException(CoachNotFoundErrorMessage);

            string nameBg = CapitalizeNames(TransliterateToBg(model.Name));
            string descBg = TransliterateToBg(model.Description);
            string natBg = Capitalize(TransliterateToBg(model.Nationality));

            try
            {
                var trName = await translator.TranslateTextAsync(model.Name, LanguageCode.English, LanguageCode.Bulgarian);
                if (!string.IsNullOrWhiteSpace(trName?.Text))
                {
                    nameBg = string.Join(" ", trName.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                             .Select(word => Capitalize(word)));
                }

                var trDesc = await translator.TranslateTextAsync(model.Description, LanguageCode.English, LanguageCode.Bulgarian);
                if (!string.IsNullOrWhiteSpace(trDesc?.Text)) descBg = trDesc.Text;

                var trNat = await translator.TranslateTextAsync($"He is {model.Nationality}", LanguageCode.English, LanguageCode.Bulgarian);
                if (!string.IsNullOrWhiteSpace(trNat?.Text))
                {
                    string rawNat = trNat.Text.Split(' ').Last().Replace(".", "");
                    natBg = Capitalize(rawNat);
                }
            }
            catch
            {
                natBg = Capitalize(natBg);
            }

            coach.Name = model.Name;
            coach.NameBg = nameBg;
            coach.Age = model.Age;
            coach.ImageUrl = model.ImageUrl;
            coach.Nationality = model.Nationality;
            coach.NationalityBg = natBg;
            coach.Description = model.Description;
            coach.DescriptionBg = descBg;

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<DeleteCoachViewModel?> GetCoachForDeletingAsync(string userId, int id)
        {
            var user = await userManager.FindByIdAsync(userId);
            bool isAdmin = await userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin || userId == null)
            {
                throw new ArgumentException("You must be an admin to delete a coach.");
            }

            var coach = await GetCoachByIdAsync(id);

            return new DeleteCoachViewModel
            {
                CoachId = coach.CoachId,
                Name = coach.Name,
                ImageUrl = coach.ImageUrl,
            };
        }

        public async Task<bool> DeletedCoachAsync(string userId, DeleteCoachViewModel model)
        {
            var coach = await dbContext.Coaches.FindAsync(model.CoachId);

            if (coach == null)
            {
                throw new ArgumentException(CoachNotFoundErrorMessage);
            }

            dbContext.Coaches.Remove(coach);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<TennisAcademyApp.Data.Models.Coach?> GetCoachByIdAsync(int? id)
        {
            if (id.HasValue)
            {
                var coach = await dbContext.Coaches
                    .FirstOrDefaultAsync(c => c.CoachId == id.Value);
                if (coach == null)
                {
                    throw new ArgumentException(CoachNotFoundErrorMessage);
                }
                return coach;
            }
            else
            {
                throw new ArgumentException(CoachCannotBeNullErrorMessage);
            }
        }
    }
}