namespace TennisAcademyApp.ViewModels.Coach
{
    public class AllCoachesViewModel
    {
        public int CoachId { get; set; }
        public string CoachName { get; set; } = null!;
        public string CoachNameBg { get; set; } 
        public string? ImageUrl { get; set; }
        public int CoachAge { get; set; }
    }
}
