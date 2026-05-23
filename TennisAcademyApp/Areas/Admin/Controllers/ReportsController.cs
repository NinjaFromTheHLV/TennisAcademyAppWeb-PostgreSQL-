using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TennisAcademyApp.Data;
using TennisAcademyApp.ViewModels.Report;
using TennisAcademyApp.ViewModels.Reports;

namespace TennisAcademyApp.Areas.Admin.Controllers
{
    public class ReportsController : AdminBaseController
    {
        private readonly TennisAcademyDbContext dbContext;

        public ReportsController(TennisAcademyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
        {
            var startPeriod = fromDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endPeriod = toDate ?? DateTime.Now;

            var model = new ReportsDashboardViewModel
            {
                FromDate = startPeriod,
                ToDate = endPeriod
            };

            model.CourtReports = await dbContext.Reservations
                .Where(r => r.IsDeleted == false && r.Date >= startPeriod && r.Date <= endPeriod)
                .GroupBy(r => r.Surface.Name)
                .Select(g => new CourtReportViewModel
                {
                    SurfaceName = g.Key,
                    TotalReservations = g.Count()
                })
                .OrderByDescending(c => c.TotalReservations)
                .ToListAsync();

            model.CoachReports = await dbContext.Reservations
                .Where(r => r.IsDeleted == false && r.Date >= startPeriod && r.Date <= endPeriod)
                .GroupBy(r => r.Coach.Name)
                .Select(g => new CoachReportViewModel
                {
                    CoachName = g.Key,
                    TotalTrainings = g.Count(),
                    TotalDurationMinutes = g.Sum(r => r.Duration)
                })
                .OrderByDescending(c => c.TotalTrainings)
                .ToListAsync();

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ExportReportToJson(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var startPeriod = fromDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endPeriod = toDate ?? DateTime.Now;

                var courtReports = await dbContext.Reservations
                    .Where(r => r.IsDeleted == false && r.Date >= startPeriod && r.Date <= endPeriod)
                    .GroupBy(r => r.Surface.Name)
                    .Select(g => new CourtReportViewModel
                    {
                        SurfaceName = g.Key,
                        TotalReservations = g.Count()
                    })
                    .OrderByDescending(c => c.TotalReservations)
                    .ToListAsync();

                var coachReports = await dbContext.Reservations
                    .Where(r => r.IsDeleted == false && r.Date >= startPeriod && r.Date <= endPeriod)
                    .GroupBy(r => r.Coach.Name)
                    .Select(g => new CoachReportViewModel
                    {
                        CoachName = g.Key,
                        TotalTrainings = g.Count(),
                        TotalDurationMinutes = g.Sum(r => r.Duration)
                    })
                    .OrderByDescending(c => c.TotalTrainings)
                    .ToListAsync();

                var exportData = new
                {
                    ReportPeriod = new { From = startPeriod.ToString("yyyy-MM-dd"), To = endPeriod.ToString("yyyy-MM-dd") },
                    ExportedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    CourtUtilizationReport = courtReports,
                    CoachWorkloadReport = coachReports
                };

                var jsonOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
                string jsonString = System.Text.Json.JsonSerializer.Serialize(exportData, jsonOptions);

                byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);
                string fileName = $"Business_Report_{startPeriod:yyyyMMdd}_to_{endPeriod:yyyyMMdd}.json";

                return File(fileBytes, "application/json", fileName);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error generating JSON report export.";
                return RedirectToAction(nameof(Index), new { fromDate, toDate });
            }
        }
    }
}