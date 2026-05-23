namespace TennisAcademyApp.ViewModels.Reservation
{
    public class ReservationDetailsViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string CoachName { get; set; } = null!;
        public string SurfaceName { get; set; } = null!;
        public string TrainingTypeName { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public string? Note { get; set; }
        public string? NoteBg { get; set; }
    }
}
