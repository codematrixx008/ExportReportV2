using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileExportController : ControllerBase
    {
        private readonly AppFileHandling _fileHandling;

        public FileExportController()
        {
            _fileHandling = new AppFileHandling();
        }

        [HttpGet("export")]
        public IActionResult Export([FromQuery] EFileType fileType)
        {
            var table = _fileHandling.CreateDummyDataTable();
            var result = _fileHandling.ConvertToFile(fileType, table);
            return File(result.Stream, result.ContentType, result.FileName);
        }
    }
}
