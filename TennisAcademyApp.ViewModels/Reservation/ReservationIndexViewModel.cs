namespace TennisAcademyApp.ViewModels.Reservation
{
    public class ReservationIndexViewModel
    {
        public int ReservationId { get; set; }

        public string CoachName { get; set; } = null!;

        public string CoachNameBg { get; set; } = null!;

        public string TrainingTypeName { get; set; } = null!;

        public string TrainingTypeNameBg { get; set; } = null!;

        public string Date { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}