using System;
using System.Collections.Generic;
using TennisAcademyApp.ViewModels.Reports;

namespace TennisAcademyApp.ViewModels.Report
{
    public class ReportsDashboardViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public IEnumerable<CourtReportViewModel> CourtReports { get; set; } = new List<CourtReportViewModel>();
        public IEnumerable<CoachReportViewModel> CoachReports { get; set; } = new List<CoachReportViewModel>();
    }
}