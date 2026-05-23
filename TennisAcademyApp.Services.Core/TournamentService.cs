using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Tournament;

namespace TennisAcademyApp.Services.Core
{
    public class TournamentService : ITournamentService
    {
        private readonly TennisAcademyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public TournamentService(TennisAcademyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<TournamentQueryViewModel> GetAllTournamentsAsync(int? categoryId = null, string? searchTerm = null, int currentPage = 1, int tournamentsPerPage = 6)
        {
            var isBg = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            var query = dbContext.Tournaments
                .Where(t => !t.IsDeleted)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearch = searchTerm.ToLower();
                query = query.Where(t => t.Title.ToLower().Contains(normalizedSearch) ||
                                      t.TitleBg.ToLower().Contains(normalizedSearch) ||
                                      t.Description.ToLower().Contains(normalizedSearch) ||
                                      t.DescriptionBg.ToLower().Contains(normalizedSearch));
            }

            int totalTournaments = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalTournaments / tournamentsPerPage);

            if (currentPage < 1) currentPage = 1;

            var tournaments = await query
                .OrderBy(t => t.StartDate)
                .Skip((currentPage - 1) * tournamentsPerPage)
                .Take(tournamentsPerPage)
                .Select(t => new TournamentAllViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    TitleBg = t.TitleBg,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    EntryFee = t.EntryFee,
                    CategoryName = t.Category.Name,
                    CategoryNameBg = t.Category.NameBg,
                    MaxParticipants = t.MaxParticipants,
                    CurrentParticipantsCount = t.Participants.Count
                })
                .ToListAsync();

            // 4. Зареждане на категориите за падащото меню
            var categories = await dbContext.TournamentCategories
                .Where(c => !c.IsDeleted)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = isBg ? c.NameBg : c.Name
                })
                .ToListAsync();

            return new TournamentQueryViewModel
            {
                CurrentPage = currentPage,
                TotalPages = totalPages,
                CategoryId = categoryId,
                SearchTerm = searchTerm,
                Categories = categories,
                Tournaments = tournaments
            };
        }

        public async Task<TournamentDetailsViewModel?> GetTournamentDetailsAsync(int id, string currentUserId)
        {
            return await dbContext.Tournaments
                .Where(t => t.Id == id && !t.IsDeleted)
                .Select(t => new TournamentDetailsViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    TitleBg = t.TitleBg,
                    Description = t.Description,
                    DescriptionBg = t.DescriptionBg,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    EntryFee = t.EntryFee,
                    MaxParticipants = t.MaxParticipants,
                    CurrentParticipantsCount = t.Participants.Count,
                    IsAlreadyEnrolled = t.Participants.Any(p => p.UserId == currentUserId)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> EnrollUserAsync(int tournamentId, string userId)
        {
            var tournament = await dbContext.Tournaments
                .Include(t => t.Participants)
                .FirstOrDefaultAsync(t => t.Id == tournamentId && !t.IsDeleted);

            if (tournament == null) return false;

            if (tournament.Participants.Count >= tournament.MaxParticipants ||
                tournament.Participants.Any(p => p.UserId == userId))
            {
                return false;
            }

            var participant = new TournamentUser
            {
                TournamentId = tournamentId,
                UserId = userId,
                EnrolledOn = DateTime.UtcNow
            };

            await dbContext.TournamentsUsers.AddAsync(participant);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnenrollUserAsync(int tournamentId, string userId)
        {
            var participant = await dbContext.TournamentsUsers
                .FirstOrDefaultAsync(tu => tu.TournamentId == tournamentId && tu.UserId == userId);

            if (participant == null)
            {
                return false;
            }

            dbContext.TournamentsUsers.Remove(participant);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}