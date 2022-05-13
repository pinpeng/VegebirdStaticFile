using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private IWebHostEnvironment _env;
        public ImageFilesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // GET api/<ImageFilesController>/5
        [HttpGet("{imageName}")]
        public IActionResult Get(string imageName)
        {
            var filePath = Path.Combine(
                _env.ContentRootPath, "MyStaticFiles", "images", imageName);

            if (System.IO.File.Exists(filePath))
            {
                return (PhysicalFile(filePath, "image/jpeg"));
            }

            return NotFound();
        }

        // PUT api/<ImageFilesController>/5
        [HttpPut("{imageName}")]
        //[Consumes("multipart/form-data")]
        async public Task<IActionResult> Put(string imageName, [FromForm] IFormFile formFile)
        {
            var filePath = Path.Combine(
            _env.ContentRootPath, "MyStaticFiles", "images", imageName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            using (var stream = System.IO.File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
            }

            return Ok();
        }

        // DELETE api/<ImageFilesController>/5
        [HttpDelete("{imageName}")]
        public void Delete(string imageName)
        {
            var filePath = Path.Combine(
            _env.ContentRootPath, "MyStaticFiles", "images", imageName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

        }
    }
}
