using System;

namespace TennisAcademyApp.ViewModels.Reservation
{
    public class ReservationExportViewModel
    {
        public int Id { get; set; }
        public string DateTime { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string CoachName { get; set; } = null!;
        public string TrainingType { get; set; } = null!;
        public string SurfaceName { get; set; } = null!;
    }
}