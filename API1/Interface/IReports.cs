using System.Data;
using API1.Model;
using Microsoft.AspNetCore.Mvc;
using Util;

namespace API1.Interface
{
    public interface IReports
    {
        Task<IEnumerable<ReportsModel>> GetAllReportsAsync();
        Task<ReportsModel> GetReportsByIdAsync(int reportId);
        Task<bool> UpdateReportGeneratingStatus(int reportId, bool isGenerating);
        Task<bool> UpdateReportGeneratedStatus(int reportId, bool isGenerated);
        Task<bool> UpdateLastGeneratedOnAndBy(int reportId, DateTime lastGeneratedOn, int lastGeneratedBy);
        Task<MemoryStream> GetCsvStreamForReport(ExportReportsModel model);
        Task<MemoryStream> GetWorkbookStreamForReport(ExportReportsModel reportModal);
        Task<DataTable> GetWorkbookStreamForReportDataTable(ExportReportsModel reportModal);
        Task<bool> GenerateReports(int ReportId, string ReportName, string SpName);
        Task<List<GeneratedFiles>> GenerateSaveAndReturnReports(int ReportId, string ReportName, string SpName);
        Task<DataTable> ExecuteQueryAndReturnDataTable(string SpName);
    }
}
