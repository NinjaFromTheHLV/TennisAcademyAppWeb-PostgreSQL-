namespace TennisAcademyApp.ViewModels.Reports
{
    public class CourtReportViewModel
    {
        public string SurfaceName { get; set; } = null!;

        public string? SurfaceNameBg { get; set; }

        public int TotalReservations { get; set; }
    }
}