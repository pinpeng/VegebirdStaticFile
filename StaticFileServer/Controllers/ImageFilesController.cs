using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StaticFileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageFilesController : ControllerBase
    {
        private IConfiguration _configuration;
        private string orgPath, filePath;
        public ImageFilesController(IConfiguration configuration)
        {
            _configuration = configuration;

            orgPath = Path.Combine(_configuration["StaticFilePath"]);
            filePath = Path.Combine(orgPath, "images");
            if (!Directory.Exists(orgPath)) Directory.CreateDirectory(orgPath);
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
        }

        // GET api/<ImageFilesController>/5
        [HttpGet("{imageName}")]
        public IActionResult Get(string imageName)
        {
            var imagePath = Path.Combine(filePath, imageName);

            if (System.IO.File.Exists(imagePath))
            {
                return (PhysicalFile(imagePath, "image/jpeg"));
            }

            return NotFound();
        }

        // PUT api/<ImageFilesController>/5
        [HttpPut("{imageName}")]
        //[Consumes("multipart/form-data")]
        async public Task<IActionResult> Put(string imageName, [FromForm] IFormFile formFile)
        {
            var imagePath = Path.Combine(filePath, imageName);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            using (var stream = System.IO.File.Create(imagePath))
            {
                await formFile.CopyToAsync(stream);
            }

            return Ok();
        }

        // DELETE api/<ImageFilesController>/5
        [HttpDelete("{imageName}")]
        public IActionResult Delete(string imageName)
        {
            var imagePath = Path.Combine(filePath, imageName);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            return Ok();
        }
    }
}
