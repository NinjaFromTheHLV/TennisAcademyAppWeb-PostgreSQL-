using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Reservation;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.User;
using static TennisAcademyApp.GCommon.Validations.ValidationConstants.Reservation;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Reservation;

namespace TennisAcademyApp.Services.Core
{
    public class ReservationService : IReservationService
    {
        private readonly TennisAcademyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDateTimeProvider dateTimeProvider;

        public ReservationService(TennisAcademyDbContext dbContext, UserManager<ApplicationUser> userManager, IDateTimeProvider dateTimeProvider)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<IEnumerable<ReservationIndexViewModel>?> GetFilteredUserReservationsAsync(string userId, string? searchTerm, DateTime? fromDate, DateTime? toDate, string? sortOrder)
        {
            await AutoReservationDeleteAsync();

            var currentCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture == "bg";

            var query = dbContext.Reservations
                .Where(r => r.PlayerId == userId && r.IsDeleted == false && r.IsCompleted == false)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(r => r.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(r => r.Date <= toDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.ToLower();
                query = query.Where(r => r.Coach.Name.ToLower().Contains(term) ||
                                         r.Coach.NameBg.ToLower().Contains(term) ||
                                         r.TrainingType.Name.ToLower().Contains(term) ||
                                         r.TrainingType.NameBg.ToLower().Contains(term));
            }

            query = sortOrder switch
            {
                "date_desc" => query.OrderByDescending(r => r.Date),
                "Duration" => query.OrderBy(r => r.Duration),
                "duration_desc" => query.OrderByDescending(r => r.Duration),
                _ => query.OrderBy(r => r.Date)
            };

            return await query
                .AsNoTracking()
                .Select(r => new ReservationIndexViewModel
                {
                    ReservationId = r.Id,
                    CoachName = isBg ? r.Coach.NameBg : r.Coach.Name,
                    TrainingTypeName = isBg ? r.TrainingType.NameBg : r.TrainingType.Name,
                    Date = r.Date.ToString(DateFormat)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReservationExportViewModel>> GetAllReservationsForExportAsync(string? searchTerm, DateTime? fromDate, DateTime? toDate)
        {
            var query = this.dbContext.Reservations
                .Where(r => r.IsDeleted == false)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(r => r.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(r => r.Date <= toDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.ToLower();
                query = query.Where(r =>
                    r.Player.Email.ToLower().Contains(term) ||
                    r.Coach.Name.ToLower().Contains(term));
            }

            return await query
                .OrderBy(r => r.Date)
                .Select(r => new ReservationExportViewModel
                {
                    Id = r.Id,
                    DateTime = r.Date.ToString("dd.MM.yyyy HH:mm"),
                    UserEmail = r.Player.Email,
                    CoachName = r.Coach.Name,
                    TrainingType = r.TrainingType.Name,
                    SurfaceName = r.Surface.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReservationIndexViewModel>?> GetUserReservationsAsync(string userId)
        {
            await AutoReservationDeleteAsync();
            var user = await userManager.FindByIdAsync(userId);

            var reservations = await dbContext.Reservations
                .AsNoTracking()
                .Include(r => r.Coach)
                .Include(r => r.Surface)
                .Include(r => r.TrainingType)
                .Where(r => r.PlayerId == userId && r.IsDeleted == false && r.IsCompleted == false)
                .Select(r => new ReservationIndexViewModel
                {
                    ReservationId = r.Id,
                    CoachName = r.Coach.Name,
                    TrainingTypeName = r.TrainingType.Name,
                    Date = r.Date.ToString(DateFormat),
                })
                .ToListAsync();

            return reservations;
        }

        public async Task<bool> CreateReservationAsync(string userId, ReservationCreateInputModel model)
        {
            bool result = false;

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException(UserCannotBeNull);
            }

            var surface = await dbContext.Surfaces.FindAsync(model.SurfaceId);
            var trainingType = await dbContext.Trainings.FindAsync(model.TrainingTypeId);
            var coach = await dbContext.Coaches.FindAsync(model.CoachId);

            bool IsAvailable = await IsCoachAvailableAtTheTimeAsync(model);
            await DateValidationAsync(model);

            if (model.Duration != 60 && model.Duration != 120)
            {
                throw new ArgumentException(DurationErrorMessage);
            }

            var currentCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture == "bg";

            var newReservation = new Reservation()
            {
                PlayerId = userId,
                SurfaceId = model.SurfaceId,
                TrainingTypeId = model.TrainingTypeId,
                CoachId = model.CoachId,
                Date = model.Date,
                Duration = model.Duration,
                Note = model.Note,
                NoteBg = isBg ? model.Note : null,
                IsCompleted = false,
                IsDeleted = false
            };

            await dbContext.Reservations.AddAsync(newReservation);
            await dbContext.SaveChangesAsync();

            result = true;
            return result;
        }

        public async Task<bool> AutoReservationDeleteAsync()
        {
            bool result = false;
            var expiredReservations = await dbContext.Reservations
                .Where(r => r.Date <= dateTimeProvider.Now && !r.IsCompleted && !r.IsDeleted)
                .ToListAsync();

            if (expiredReservations.Any())
            {
                foreach (var r in expiredReservations)
                {
                    r.IsCompleted = true;
                }

                await dbContext.SaveChangesAsync();
                return true;
            }
            return result;
        }

        public async Task<ReservationDetailsViewModel?> GetUserReservationDetailsAsync(string userId, int? id)
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture == "bg";

            ReservationDetailsViewModel? details = null;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException(UserCannotBeNull);
            }

            if (id.HasValue)
            {
                var reservationDetails = await dbContext.Reservations
                    .AsNoTracking()
                    .Include(r => r.Coach)
                    .Include(r => r.Surface)
                    .Include(r => r.TrainingType)
                    .FirstOrDefaultAsync(r => r.Id == id && r.PlayerId == userId);

                if (reservationDetails == null)
                {
                    throw new ArgumentException(ReservationNotFoundErrorMessage);
                }

                details = new ReservationDetailsViewModel
                {
                    Id = reservationDetails.Id,
                    ImageUrl = reservationDetails.Surface.ImageUrl,
                    Duration = reservationDetails.Duration,
                    Date = reservationDetails.Date,
                    CoachName = isBg ? reservationDetails.Coach.NameBg : reservationDetails.Coach.Name,
                    SurfaceName = isBg ? reservationDetails.Surface.NameBg : reservationDetails.Surface.Name,
                    TrainingTypeName = isBg ? reservationDetails.TrainingType.NameBg : reservationDetails.TrainingType.Name,
                    Note = reservationDetails.Note,
                    NoteBg = reservationDetails.NoteBg
                };
            }
            return details;
        }

        public async Task<ReservationDeleteViewModel?> GetUserReservationForDeletingAsync(string userId, int? id)
        {
            ReservationDeleteViewModel? reservationToDelete = null;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException(UserCannotBeNull);
            }

            if (id.HasValue)
            {
                var reservation = await dbContext.Reservations
                    .AsNoTracking()
                    .Include(r => r.Coach)
                    .Include(r => r.Surface)
                    .FirstOrDefaultAsync(r => r.Id == id && r.PlayerId == userId);

                if (reservation == null)
                {
                    throw new ArgumentException(ReservationNotFoundErrorMessage);
                }

                var currentCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                bool isBg = currentCulture == "bg";

                if (reservation.PlayerId == userId)
                {
                    reservationToDelete = new ReservationDeleteViewModel
                    {
                        Id = reservation.Id,
                        SurfaceName = isBg ? reservation.Surface.NameBg : reservation.Surface.Name,
                        Date = reservation.Date,
                        ImageUrl = reservation.Surface.ImageUrl
                    };
                }
            }
            return reservationToDelete;
        }

        public async Task<bool> DeleteReservationAsync(string userId, ReservationDeleteViewModel model)
        {
            bool result = false;

            var user = await userManager.FindByIdAsync(userId);
            var reservation = await dbContext.Reservations.FindAsync(model.Id);
            if (user == null)
            {
                throw new ArgumentException(UserCannotBeNull);
            }
            if (reservation == null)
            {
                throw new ArgumentException(ReservationNotFoundErrorMessage);
            }

            if (reservation.PlayerId == userId)
            {
                reservation.IsDeleted = true;
                reservation.IsCompleted = false;
                await dbContext.SaveChangesAsync();
            }

            result = true;
            return result;
        }

        public async Task<IEnumerable<ReservationHistoryViewModel>?> GetUserReservationHistoryAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture == "bg";

            var pastReservations = await dbContext.Reservations
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Include(r => r.Coach)
                .Include(r => r.Surface)
                .Include(r => r.TrainingType)
                .Where(r => r.PlayerId == userId && (r.IsDeleted || r.IsCompleted))
                .Select(r => new ReservationHistoryViewModel
                {
                    ReservationId = r.Id,
                    CoachName = isBg ? r.Coach.NameBg : r.Coach.Name,
                    TrainingTypeName = isBg ? r.TrainingType.NameBg : r.TrainingType.Name,
                    SurfaceName = isBg ? r.Surface.NameBg : r.Surface.Name,
                    SurfaceImageUrl = r.Surface.ImageUrl,
                    IsCanceled = r.IsDeleted
                })
                .ToListAsync();

            return pastReservations;
        }

        public async Task DateValidationAsync(ReservationCreateInputModel model)
        {
            if (model.Date < dateTimeProvider.Now)
            {
                throw new ArgumentException(PastDateErrorMessage);
            }
            if (model.Date < dateTimeProvider.Now.AddHours(2))
            {
                throw new ArgumentException(TwoHoursErrorMessage);
            }
            if (model.Date > dateTimeProvider.Now.AddDays(60))
            {
                throw new ArgumentException(FutureDateErrorMessage);
            }
            if (model.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new ArgumentException(SundayErrorMessage);
            }
            if (model.Date.TimeOfDay < TimeSpan.FromHours(8)
               || model.Date.AddMinutes(model.Duration).TimeOfDay > TimeSpan.FromHours(20))
            {
                throw new ArgumentException(SelectedTimeErrorMessage);
            }
            await Task.CompletedTask;
        }

        public async Task<bool> IsCoachAvailableAtTheTimeAsync(ReservationCreateInputModel model)
        {
            bool result = false;
            var endDate = model.Date.AddMinutes(model.Duration);

            bool existingReservation = await dbContext.Reservations
                .AsNoTracking()
                .AnyAsync(r =>
                    r.CoachId == model.CoachId &&
                    !r.IsDeleted &&
                    r.Date < endDate &&
                    r.Date.AddMinutes(r.Duration) > model.Date);

            if (existingReservation)
            {
                throw new ArgumentException(CoachNotAvailableErrorMessage);
            }
            else
            {
                result = true;
            }
            return result;
        }
    }
}