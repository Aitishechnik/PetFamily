using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.FileProvider;
using PetFamily.Application.TestMinio;

namespace PetFamily.API.Controllers
{
    public class MinioTestController : ApplicationController
    {
        [HttpPost]
        public async Task<IActionResult> UploadFile(
            IFormFile file,
            [FromServices] AddFileHandler handler,
            CancellationToken cancellationToken = default)
        {
            await using var stream = file.OpenReadStream();

            var path = Guid.NewGuid().ToString();

            var fileData = new FileData(stream, "photos", path);

            await handler.Handle(fileData);

            return Ok("Minio Test Endpoint");
        }
    }
}
