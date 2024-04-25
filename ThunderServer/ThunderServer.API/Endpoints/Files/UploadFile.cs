using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace ThunderServer.API.Endpoints.Files
{
    public class UploadFile : EndpointBaseAsync.WithRequest<FileUploadRequest>.WithActionResult
    {
        [HttpPost("/api/files/Upload")]
        public override async Task<ActionResult> HandleAsync(FileUploadRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();

            return Ok();
        }
    }
}
