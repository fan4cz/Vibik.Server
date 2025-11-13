using Api.Application.Features.Photos.UploadPhoto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Features.Photos;

[ApiController]
[Route("photos")]
public class PhotosController(IMediator mediator) : ControllerBase
{
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadPhoto([FromForm] UploadPhotoRequest request)
    {
        var result = await mediator.Send(new UploadPhotoCommand(request.File));

        return Created(result, new { result });
    }
}