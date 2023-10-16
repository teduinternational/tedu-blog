using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using TeduBlog.Core.ConfigOptions;

namespace TeduBlog.Api.Controllers.AdminApi
{
    [Route("api/admin/media")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly MediaSettings _settings;

        public MediaController(IWebHostEnvironment env, IOptions<MediaSettings> settings)
        {
            _hostingEnv = env;
            _settings = settings.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult UploadImage(string type)
        {
            var allowImageTypes = _settings.AllowImageFileTypes?.Split(",");

            var now = DateTime.Now;
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return null;
            }

            var file = files[0];
            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition)?.FileName?.Trim('"');
            if (allowImageTypes?.Any(x => filename?.EndsWith(x, StringComparison.OrdinalIgnoreCase) == true) == false)
            {
                throw new Exception("Không cho phép tải lên file không phải ảnh.");
            }

            var imageFolder = $@"\{_settings.ImageFolder}\images\{type}\{now:MMyyyy}";

            var folder = _hostingEnv.WebRootPath + imageFolder;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var filePath = Path.Combine(folder, filename);
            using (var fs = global::System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            var path = Path.Combine(imageFolder, filename).Replace(@"\", @"/");
            return Ok(new { path });
        }
    }
}
