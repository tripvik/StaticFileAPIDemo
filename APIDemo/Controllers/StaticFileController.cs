using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace APIDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StaticFileController : ControllerBase
    {
        private readonly ILogger<StaticFileController> _logger;
        private readonly IWebHostEnvironment _env;

        public StaticFileController(ILogger<StaticFileController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        [HttpGet("{fileName}")]
        public IActionResult Get(string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var stream = System.IO.File.OpenRead(filePath);
            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = fileName
            };
        }
    }
}
