using TennisAcademyApp.ViewModels.Tournament;

namespace TennisAcademyApp.Services.Core.Contracts
{
    public interface ITournamentService
    {
        Task<TournamentQueryViewModel> GetAllTournamentsAsync(int? categoryId = null, string? searchTerm = null, int currentPage = 1, int tournamentsPerPage = 6);

        Task<TournamentDetailsViewModel?> GetTournamentDetailsAsync(int id, string currentUserId);

        Task<bool> EnrollUserAsync(int tournamentId, string userId);
        Task<bool> UnenrollUserAsync(int tournamentId, string userId);
    }
}