namespace TennisAcademyApp.ViewModels.Coach
{
    public class CoachScheduleViewModel
    {
        public int ReservationId { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public string? Note { get; set; }
        public string? NoteBg { get; set; }

        public string SurfaceName { get; set; } = null!;
        public string SurfaceNameBg { get; set; } = null!;
        public string TrainingTypeName { get; set; } = null!;
        public string TrainingTypeNameBg { get; set; } = null!;

        public string PlayerName { get; set; } = null!;
        public string PlayerEmail { get; set; } = null!;
    }
}