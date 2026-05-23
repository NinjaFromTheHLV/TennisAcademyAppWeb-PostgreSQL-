namespace TennisAcademyApp.ViewModels.Reservation
{
    public class ReservationHistoryViewModel
    {
        public int ReservationId { get; set; }

        public string CoachName { get; set; } = null!;

        public string? CoachNameBg { get; set; }

        public string TrainingTypeName { get; set; } = null!;

        public string? TrainingTypeNameBg { get; set; }

        public string SurfaceImageUrl { get; set; } = null!;

        public string SurfaceName { get; set; } = null!;

        public string Date { get; set; } = null!;

        public bool IsDeleted { get; set; }
        public bool IsCanceled { get; set; }
    }
}