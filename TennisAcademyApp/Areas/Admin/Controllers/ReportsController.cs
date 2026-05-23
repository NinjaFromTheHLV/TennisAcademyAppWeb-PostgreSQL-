using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Threading.Tasks;
using TennisAcademyApp.Services.Interfaces;
using TennisAcademyApp.ViewModels.Report;

namespace TennisAcademyApp.Areas.Admin.Controllers
{
    public class ReportsController : AdminBaseController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
        {
            var model = await _reportService.GetReportsAsync(fromDate, toDate);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ExportReportToJson(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                string jsonString = await _reportService.GetJsonReportAsync(fromDate, toDate);
                byte[] fileBytes = Encoding.UTF8.GetBytes(jsonString);

                var startPeriod = fromDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endPeriod = toDate ?? DateTime.Now;
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