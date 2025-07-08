using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.FileManagement.Add;
using PetFamily.Application.FileManagement.Delete;
using PetFamily.Application.FileManagement.Presign;

namespace PetFamily.API.Controllers
{
    //public class MinioTestController : ApplicationController
    //{
    //    [HttpPost]
    //    public async Task<IActionResult> UploadFiles(
    //        IFormFileCollection files,
    //        [FromServices] AddFilesHandler handler,
    //        CancellationToken cancellationToken = default)
    //    {
    //        await using var fileProcessor = new FormFileProcessor();
    //        var createFilesDTO = fileProcessor.Process(files);

    //        var fileName = Guid.NewGuid().ToString();

    //        var request = new AddFilesRequest(createFilesDTO);

    //        var result = await handler.Handle(
    //            request,
    //            cancellationToken);

    //        if (result.IsFailure)
    //            return result.Error.ToResponse();

    //        return Ok(result.Value);
    //    }

    //    [HttpDelete]
    //    public async Task<IActionResult> RemoveFile(
    //        [FromQuery] string objectName,
    //        [FromServices] DeleteFileHandler handler,
    //        CancellationToken cancellationToken = default)
    //    {
    //        var request = new DeleteFileRequest(objectName);

    //        var result = await handler.Handle(request, cancellationToken);
    //        if (result.IsFailure)
    //        {
    //            return result.Error.ToResponse();
    //        }

    //        return Ok(result.Value);
    //    }

    //    [HttpGet("link")]
    //    public async Task<IActionResult> PresignedLink(
    //        [FromQuery] string objectName,
    //        [FromServices] GetPresignedHandler handler,
    //        CancellationToken cancellationToken = default)
    //    {
    //        var request = new GetPresignedRequest(objectName);

    //        var result = await handler.Handle(request, cancellationToken);

    //        if (result.IsFailure)
    //        {
    //            return result.Error.ToResponse();
    //        }

    //        return Ok(result.Value);
    //    }
    //}
}
