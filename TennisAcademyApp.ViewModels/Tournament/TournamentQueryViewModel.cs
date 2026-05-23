using Microsoft.AspNetCore.Mvc.Rendering;

namespace TennisAcademyApp.ViewModels.Tournament
{
    public class TournamentQueryViewModel
    {
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int? CategoryId { get; set; }
        public string? SearchTerm { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<TournamentAllViewModel> Tournaments { get; set; } = new List<TournamentAllViewModel>();
    }
}