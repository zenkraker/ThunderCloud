using Microsoft.AspNetCore.Mvc;

namespace ThunderServer.API.Endpoints.Files
{
    public sealed record FileUploadRequest
    {
        [FromForm]
        public required List<IFormFile> Files { get; init; }

        [FromQuery]
        public string? PathToStoreFiles { get; init; }
    }
}