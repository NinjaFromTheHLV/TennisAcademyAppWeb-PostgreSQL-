using System;
using System.Threading.Tasks;
using TennisAcademyApp.ViewModels.Report;

namespace TennisAcademyApp.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportsDashboardViewModel> GetReportsAsync(DateTime? fromDate, DateTime? toDate);
        Task<string> GetJsonReportAsync(DateTime? fromDate, DateTime? toDate);
    }
}