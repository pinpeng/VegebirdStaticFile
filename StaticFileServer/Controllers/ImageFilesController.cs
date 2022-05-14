using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StaticFileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageFilesController : ControllerBase
    {
        private IConfiguration _configuration;
        private string orgPath;
        public ImageFilesController(IConfiguration configuration)
        {
            _configuration = configuration;

            orgPath = Path.Combine(_configuration["StaticFilePath"]);
            if (!Directory.Exists(orgPath)) Directory.CreateDirectory(orgPath);
        }

        // GET api/<ImageFilesController>/5
        [HttpGet("{groupName}/{imageName}")]
        public IActionResult Get(string groupName,string imageName)
        {
            var imagePath = Path.Combine(orgPath, groupName);
            if (!Directory.Exists(imagePath)) Directory.CreateDirectory(imagePath);

            imagePath = Path.Combine(imagePath, imageName);
            if (System.IO.File.Exists(imagePath))
            {
                return (PhysicalFile(imagePath, "image/jpeg"));
            }

            return NotFound();
        }

        // PUT api/<ImageFilesController>/5
        [HttpPut("{groupName}/{imageName}")]
        //[Consumes("multipart/form-data")]
        async public Task<IActionResult> Put(
            string groupName,string imageName, [FromForm] IFormFile formFile)
        {
            var imagePath = Path.Combine(orgPath,groupName);
            if (!Directory.Exists(imagePath)) Directory.CreateDirectory(imagePath);

            imagePath = Path.Combine(imagePath, imageName);

            if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);

            using (var stream = System.IO.File.Create(imagePath))
            {
                await formFile.CopyToAsync(stream);
            }

            return Ok();
        }

        // DELETE api/<ImageFilesController>/5
        [HttpDelete("{groupName}/{imageName}")]
        public IActionResult Delete(string groupName, string imageName)
        {
            var imagePath = Path.Combine(orgPath, groupName);
            if (!Directory.Exists(imagePath)) return NotFound();

            imagePath = Path.Combine(imagePath, imageName);

            if (!System.IO.File.Exists(imagePath)) return NotFound();

            System.IO.File.Delete(imagePath);
            return Ok();
        }
    }
}
