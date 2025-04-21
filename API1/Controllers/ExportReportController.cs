using Microsoft.AspNetCore.Mvc;
using API1.Interface;
using API1.Model;
using OfficeOpenXml;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Text;
using API1.Repository;
using System.IO.Compression;
using System.IO.Pipes;
using Microsoft.Extensions.Logging;
using Dapper;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using API1;
using System.Xml.Linq;


[ApiController]
[Route("[controller]")]
public class ReportsController : ControllerBase
{    
    DateTime dati = new DateTime();
    Stopwatch sw = new Stopwatch();
    private readonly IReports _reportsRepository;
    private readonly ILogger<ReportsController> _logger;
    private readonly IDapperDbConnection _dapperDbConnection;
    private readonly string _connectionString;
    private readonly string _reportPath;
    private readonly AppFileHandling fileHandling;


    public ReportsController(ILogger<ReportsController> logger, IReports reportsRepository,IDapperDbConnection dapperDbConnection, IConfiguration configuration)
    {
        _logger = logger;
        _reportsRepository = reportsRepository;
        _dapperDbConnection = dapperDbConnection;
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        _reportPath = configuration["ReportPath:Path"] ?? string.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReportsModel>>> GetAllReports()
    {
        var _reports = await _reportsRepository.GetAllReportsAsync();
        return Ok(_reports);
    }

    //[HttpGet]
    public async Task<ReportsModel> GetReportsByreportIdAsync(int reportId)
    {
        var _reports = await _reportsRepository.GetReportsByIdAsync(reportId);
        return _reports;
    }



    [HttpGet("DownloadReportFile")]
    public async Task<IActionResult> DownloadReportFile(int reportId, string type)
    {
        // Create a new DataTable
        DataTable table = new DataTable("SampleTable");

        // Add columns
        table.Columns.Add("ID", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("DOB", typeof(DateTime));
        table.Columns.Add("IsActive", typeof(bool));

        // Add rows
        table.Rows.Add(1, "John Doe", new DateTime(1990, 5, 15), true);
        table.Rows.Add(2, "Jane Smith", new DateTime(1985, 12, 22), false);
        table.Rows.Add(3, "Mike Johnson", new DateTime(2000, 8, 3), true);


        var fh = new AppFileHandling();
        if (!Enum.TryParse(type, true, out EFileType fileType))
        {
            return BadRequest("Invalid file type.");
        }

        FileResultModel result = new FileResultModel();
        result = fh.ConvertToFile(fileType, table);

        return result.FileContentResult;

    }

    //[HttpGet("DownloadReportFile")]
    //public async Task<IActionResult> DownloadReportFile(int reportId, string type)
    //{
    //    var reports = await _reportsRepository.GetReportsByIdAsync(reportId);

    //    if (reports == null)
    //    {
    //        return NotFound("Reports not found.");
    //    }

    //    FileResultModel result = new FileResultModel();
    //    string exportType = "stream";
    //    if (exportType == "stream")
    //    {
    //        DataTable dt = await _reportsRepository.ExecuteQueryAndReturnDataTable(reports.SpName);
    //        if (!Enum.TryParse(type, true, out EFileType fileType))
    //        {
    //            return BadRequest("Invalid file type.");
    //        }
    //        result = fileHandling.ConvertToFile(fileType, dt);
    //    }
    //    else if(exportType == "saveToDisk")
    //    {
    //        Boolean blnCreateNewFiles = false;
    //        string repName = $"{_reportPath}/{reports.ReportFileName}";
    //        if (!System.IO.File.Exists(repName))
    //            blnCreateNewFiles = true;

    //        if (blnCreateNewFiles)//First save itno disk folder and return the requested file type
    //        {
    //            var generatedFiles = await _reportsRepository.GenerateSaveAndReturnReports(reports.ReportID, reports.ReportName, reports.SpName);
    //            if (generatedFiles != null)
    //            {
    //                repName = generatedFiles.Where(s => s.ReportType == type).Select(t=>t.ReportName).FirstOrDefault() ?? string.Empty;
    //            }
    //        }

    //        byte[] fileBytes = System.IO.File.ReadAllBytes(repName);
    //        return File(fileBytes, result.ContentType, result.FileName);
    //    }

    //    return File(result.Stream.ToArray(), result.ContentType, result.FileName);
    //}


    private async Task<(MemoryStream stream, string fileName, string contentType)> ExportStreamFileByExportType(ExportReportsModel model)
    {
        MemoryStream stream;
        string fileName;
        string contentType;

        if (model.ExportType.Equals("xlsx", StringComparison.OrdinalIgnoreCase))
        {
            stream = await _reportsRepository.GetWorkbookStreamForReport(model);
            fileName = $"{model.ReportName}.xlsx";
            contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }
        else if (model.ExportType.Equals("csv", StringComparison.OrdinalIgnoreCase))
        {
            stream = await _reportsRepository.GetCsvStreamForReport(model);
            fileName = $"{model.ReportName}.csv";
            contentType = "text/csv";
        }
        else if (model.ExportType.Equals("zip", StringComparison.OrdinalIgnoreCase))
        {
            stream = new MemoryStream();
            contentType = "application/zip";
            fileName = $"{model.ReportName}_{DateTime.Now:yyyyMMdd_hhmmss}.zip";

            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                // XLSX file
                using (var xlsxStream = await _reportsRepository.GetWorkbookStreamForReport(model))
                {
                    var xlsxEntry = archive.CreateEntry($"{model.ReportName}.xlsx", System.IO.Compression.CompressionLevel.Fastest);
                    using (var entryStream = xlsxEntry.Open())
                    {
                        xlsxStream.Seek(0, SeekOrigin.Begin);
                        await xlsxStream.CopyToAsync(entryStream);
                    }
                }

                // CSV file
                using (var csvStream = await _reportsRepository.GetCsvStreamForReport(model))
                {
                    var csvEntry = archive.CreateEntry($"{model.ReportName}.csv", System.IO.Compression.CompressionLevel.Fastest);
                    using (var entryStream = csvEntry.Open())
                    {
                        csvStream.Seek(0, SeekOrigin.Begin);
                        await csvStream.CopyToAsync(entryStream);
                    }
                }
            }

            stream.Seek(0, SeekOrigin.Begin);
        }
        else
        {
            // ❌ Can't return BadRequest, so throw an exception instead
            throw new InvalidOperationException("Unsupported file type. Please choose either 'xlsx', 'csv', or 'zip'.");
        }

        return (stream, fileName, contentType);
    }


    private void TraceExecutionTime(string traceFunction, bool start = false, bool stop = false, bool restart = false)
    {
        if (start)
        {
            sw.Start();
        }
        else if (stop || restart)
        {
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Debug.WriteLine($"Execution Time for {traceFunction} {ts.TotalMilliseconds} ms started at {dati.ToString()}");
            if (restart)
            {
                sw.Restart();
            }
        }
    }


    //[HttpGet("SaveToServerForStaticReport")]
    //public async Task<IActionResult> SaveToServerForStaticReport([FromQuery] int ReportId, [FromQuery] string ReportName, [FromQuery] string SpName, [FromQuery] string ExportType)
    //{
    //    try
    //    {
    //        var model = new ExportReportsModel{ReportId = ReportId,ReportName = ReportName,SpName = SpName,ExportType = ExportType};

    //        (MemoryStream stream, string fileName, string contentType) = await ExportStreamFileByExportType(model);

    //        var folderPath = "C:\\Test";

    //        if (!Directory.Exists(folderPath))
    //        {
    //            Directory.CreateDirectory(folderPath);
    //        }

    //        var filePath = Path.Combine(folderPath, fileName);

    //        await System.IO.File.WriteAllBytesAsync(filePath, stream.ToArray());

    //        Debug.WriteLine($"Single file saves at :{DateTime.Now.ToString()}");

    //        var generatedFiles = await _reportsRepository.GenerateReports(ReportId, ReportName, SpName);

    //        Debug.WriteLine($"All format saved at :{DateTime.Now.ToString()}");
    //        //return Ok(new { message = "All Format saved successfully.", path = filePath });
    //        return Ok();
    //    }
    //    catch (Exception ex)
    //    {
    //        await _reportsRepository.UpdateReportGeneratingStatus(ReportId, false);
    //        return StatusCode(500, new { message = "Error saving file.", error = ex.Message });
    //    }
    //}

    [HttpGet("SaveToServerForStaticReport")]
    public async Task<IActionResult> SaveToServerForStaticReport(int ReportId, string ReportName,string SpName,string ExportType)
    {
        try
        {
            var model = new ExportReportsModel { ReportId = ReportId, ReportName = ReportName, SpName = SpName, ExportType = ExportType };

            (MemoryStream stream, string fileName, string contentType) = await ExportStreamFileByExportType(model);

            var folderPath = "C:\\Test";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            await System.IO.File.WriteAllBytesAsync(filePath, stream.ToArray());

            Debug.WriteLine($"Single file saves at :{DateTime.Now.ToString()}");

            var generatedFiles = await _reportsRepository.GenerateReports(ReportId, ReportName, SpName);

            Debug.WriteLine($"All format saved at :{DateTime.Now.ToString()}");
            //return Ok(new { message = "All Format saved successfully.", path = filePath });
            return Ok();
        }
        catch (Exception ex)
        {
            await _reportsRepository.UpdateReportGeneratingStatus(ReportId, false);
            return StatusCode(500, new { message = "Error saving file.", error = ex.Message });
        }
    }

    private IActionResult downloffile(string fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest(new { message = "File name is required." });
            }


            var filePath = Path.Combine(_reportPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { message = "File not found." });
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = GetContentType(filePath);

            return File(fileBytes, contentType, fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error downloading file.", error = ex.Message });
        }
    }

    private string GetContentType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".csv" => "text/csv",
            ".zip" => "application/zip",
            _ => "application/octet-stream",
        };
    }
    
    [HttpGet("DownloadFile")]
    public IActionResult DownloadFile()
    {
        if (!System.IO.File.Exists(_reportPath))
        {
            return BadRequest(new { message = "File path is not set or file does not exist." });
        }
        byte[] fileBytes = System.IO.File.ReadAllBytes(_reportPath);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DownloadedFile.xlsx");
    }
}