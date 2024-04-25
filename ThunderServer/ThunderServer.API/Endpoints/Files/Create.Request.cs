using Microsoft.AspNetCore.Mvc;

namespace ThunderServer.API.Endpoints.Files
{
    public class FileUploadRequest
    {
        [FromForm]
        public required List<IFormFile> Files { get; init; }

        [FromQuery]
        public string? PathToStoreFiles { get; init; }
    }
}