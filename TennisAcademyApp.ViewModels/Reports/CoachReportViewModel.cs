namespace TennisAcademyApp.ViewModels.Report
{
    public class CoachReportViewModel
    {
        public string CoachName { get; set; } = null!;
        public string CoachNameBg { get; set; }
        public int TotalTrainings { get; set; }
        public int TotalDurationMinutes { get; set; }
    }
}