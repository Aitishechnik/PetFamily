using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.TestMinio.Add;
using PetFamily.Application.TestMinio.Delete;
using PetFamily.Application.TestMinio.Presign;

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
            var fileName = Guid.NewGuid().ToString();

            var request = new AddFileRequest(stream, fileName);

            var result = await handler.Handle(
                request,
                cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFile(
            [FromQuery] string objectName,
            [FromServices] DeleteFileHandler handler,
            CancellationToken cancellationToken = default)
        {
            var request = new DeleteFileRequest(objectName);

            var result = await handler.Handle(request, cancellationToken);
            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet("link")]
        public async Task<IActionResult> PresignedLink(
            [FromQuery] string objectName,
            [FromServices] GetPresignedHandler handler,
            CancellationToken cancellationToken = default)
        {
            var request = new GetPresignedRequest(objectName);

            var result = await handler.Handle(request, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }
    }
}
