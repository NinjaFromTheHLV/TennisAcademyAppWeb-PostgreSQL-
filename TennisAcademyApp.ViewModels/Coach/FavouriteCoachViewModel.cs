namespace TennisAcademyApp.ViewModels.Coach
{
    public class FavouriteCoachViewModel
    {
        public int CoachId { get; set; }

        public string CoachName { get; set; } = null!;

        public string? CoachNameBg { get; set; }

        public int CoachAge { get; set; }

        public string Description { get; set; } = null!;

        public string? DescriptionBg { get; set; }

        public string ImageUrl { get; set; } = null!;
    }
}