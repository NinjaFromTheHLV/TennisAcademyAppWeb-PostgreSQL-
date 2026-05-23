using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TennisAcademyApp.Data;
using TennisAcademyApp.Services.Interfaces;
using TennisAcademyApp.ViewModels.Report;
using TennisAcademyApp.ViewModels.Reports;

namespace TennisAcademyApp.Services
{
    public class ReportService : IReportService
    {
        private readonly TennisAcademyDbContext _dbContext;

        public ReportService(TennisAcademyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReportsDashboardViewModel> GetReportsAsync(DateTime? fromDate, DateTime? toDate)
        {
            var startPeriod = fromDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endPeriod = toDate ?? DateTime.Now;

            var model = new ReportsDashboardViewModel
            {
                FromDate = startPeriod,
                ToDate = endPeriod
            };

            model.CourtReports = await _dbContext.Reservations
                .Where(r => r.IsDeleted == false && r.Date >= startPeriod && r.Date <= endPeriod)
                .GroupBy(r => new { r.Surface.Name, r.Surface.NameBg })
                .Select(g => new CourtReportViewModel
                {
                    SurfaceName = g.Key.Name,
                    SurfaceNameBg = g.Key.NameBg,
                    TotalReservations = g.Count()
                })
                .OrderByDescending(c => c.TotalReservations)
                .ToListAsync();

            model.CoachReports = await _dbContext.Reservations
                .Where(r => r.IsDeleted == false && r.Date >= startPeriod && r.Date <= endPeriod)
                .GroupBy(r => new { r.Coach.Name, r.Coach.NameBg })
                .Select(g => new CoachReportViewModel
                {
                    CoachName = g.Key.Name,
                    CoachNameBg = g.Key.NameBg,
                    TotalTrainings = g.Count(),
                    TotalDurationMinutes = g.Sum(r => r.Duration)
                })
                .OrderByDescending(c => c.TotalTrainings)
                .ToListAsync();

            return model;
        }

        public async Task<string> GetJsonReportAsync(DateTime? fromDate, DateTime? toDate)
        {
            var startPeriod = fromDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endPeriod = toDate ?? DateTime.Now;

            var data = await GetReportsAsync(fromDate, toDate);

            var exportData = new
            {
                ReportPeriod = new { From = startPeriod.ToString("yyyy-MM-dd"), To = endPeriod.ToString("yyyy-MM-dd") },
                ExportedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                CourtUtilizationReport = data.CourtReports,
                CoachWorkloadReport = data.CoachReports
            };

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(exportData, jsonOptions);
        }
    }
}