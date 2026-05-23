namespace TennisAcademyApp.ViewModels.Coach
{
    public class PaginatedCoachesViewModel
    {
        public IEnumerable<AllCoachesViewModel> Coaches { get; set; } = new List<AllCoachesViewModel>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string? SearchQuery { get; set; }
    }
}
