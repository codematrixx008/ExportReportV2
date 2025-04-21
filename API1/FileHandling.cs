using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace API1
{
    public enum EFileType
    {
        Xlsx = 1,
        Pdf = 2,
        Csv = 3,
        Zip = 4
    }

    public class FileResultModel
    {
        public MemoryStream Stream { get; set; }
        public FileContentResult FileContentResult { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }

    public class AppFileHandling
    {
        public void GetFileByPath(string filePath)
        {
            if (File.Exists(filePath))
            {
                Console.WriteLine("File found at: " + filePath);
            }
            else
            {
                Console.WriteLine("File not found at: " + filePath);
            }
        }

        public FileResultModel ConvertToFile(EFileType fileType, DataTable dataTable)
        {
            return fileType switch
            {
                EFileType.Xlsx => ConvertToXlsx(dataTable),
                EFileType.Pdf => ConvertToPdf(dataTable),
                EFileType.Csv => ConvertToCsv(dataTable),
                EFileType.Zip => ConvertToZip(dataTable),
                _ => throw new ArgumentOutOfRangeException(nameof(fileType), $"Unsupported file type: {fileType}")
            };
        }

        public DataTable CreateDummyDataTable()
        {
            DataTable dt = new DataTable("DummyTable");
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Date", typeof(DateTime));

            for (int i = 1; i <= 5; i++)
            {
                dt.Rows.Add(i, "Name " + i, DateTime.Now.AddDays(-i));
            }

            return dt;
        }

        private MemoryStream ConvertToStream(DataTable table)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

            foreach (DataColumn column in table.Columns)
            {
                writer.Write(column.ColumnName + ",");
            }

            writer.WriteLine();

            foreach (DataRow row in table.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    writer.Write(item.ToString() + ",");
                }
                writer.WriteLine();
            }

            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        private FileResultModel ConvertToPdf(DataTable dataTable)
        {
            var stream = ConvertToStream(dataTable);
            return new FileResultModel
            {
                Stream = stream,
                FileName = "data.pdf",
                ContentType = "application/pdf"
            };
        }

        private FileResultModel ConvertToCsv(DataTable dataTable)
        {
            string fileName = $"{Guid.NewGuid()}_{DateTime.Now.ToString()}.csv";
            string delimiter = ",";

            if (dataTable == null || dataTable.Columns.Count == 0)
                throw new ArgumentException("DataTable is null or empty.");

            StringBuilder sb = new StringBuilder();

            // Column headers
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                sb.Append(dataTable.Columns[i].ColumnName);
                if (i < dataTable.Columns.Count - 1)
                    sb.Append(delimiter);
            }
            sb.AppendLine();

            // Data rows
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var value = row[i].ToString().Replace("\"", "\"\"");
                    sb.Append($"\"{value}\"");
                    if (i < dataTable.Columns.Count - 1)
                        sb.Append(delimiter);
                }
                sb.AppendLine();
            }

            byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());

            // Return FileContentResult (this is okay even in a repository)
            var _FileContentResult =  new FileContentResult(buffer, "text/csv")
            {
                FileDownloadName = fileName
            };


            return new FileResultModel
            {
                FileContentResult = _FileContentResult,
                FileName = fileName,
                ContentType = "text/csv"
            };
        }

        private FileResultModel ConvertToXlsx(DataTable dataTable)
        {
            string fileName = $"{Guid.NewGuid()}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            if (dataTable == null || dataTable.Columns.Count == 0)
                throw new ArgumentException("DataTable is null or empty.");

            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Load headers
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dataTable.Columns[col].ColumnName;
                    worksheet.Cells[1, col + 1].Style.Font.Bold = true;
                }

                // Load data
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                    }
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                byte[] buffer = package.GetAsByteArray();

                var _FileContentResult = new FileContentResult(buffer,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = fileName
                };

                return new FileResultModel
                {
                    FileContentResult = _FileContentResult,
                    FileName = fileName,
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                };
            }
        }

        private FileResultModel ConvertToZip(DataTable dataTable)
        {
            string zipFileName = $"{Guid.NewGuid()}_{DateTime.Now:yyyyMMdd_HHmmss}.zip";

            if (dataTable == null || dataTable.Columns.Count == 0)
                throw new ArgumentException("DataTable is null or empty.");

            using var zipStream = new MemoryStream();

            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                // 1. CSV
                var csvFile = ConvertToCsv(dataTable);
                var csvEntry = archive.CreateEntry("report.csv");
                using (var entryStream = csvEntry.Open())
                {
                    entryStream.Write(csvFile.FileContentResult.FileContents, 0, csvFile.FileContentResult.FileContents.Length);
                }

                // 2. Excel
                var xlsxFile = ConvertToXlsx(dataTable);
                var xlsxEntry = archive.CreateEntry("report.xlsx");
                using (var entryStream = xlsxEntry.Open())
                {
                    entryStream.Write(xlsxFile.FileContentResult.FileContents, 0, xlsxFile.FileContentResult.FileContents.Length);
                }
            }

            zipStream.Position = 0;
            byte[] buffer = zipStream.ToArray();

            var _FileContentResult = new FileContentResult(buffer, "application/zip")
            {
                FileDownloadName = zipFileName
            };

            return new FileResultModel
            {
                FileContentResult = _FileContentResult,
                FileName = zipFileName,
                ContentType = "application/zip"
            };
        }

    }
}
