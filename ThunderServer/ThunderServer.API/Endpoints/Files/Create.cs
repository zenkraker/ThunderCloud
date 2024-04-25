using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using ThunderServer.API.Domain;
using ThunderServer.API.Storage;

namespace ThunderServer.API.Endpoints.Files
{
    public class Create : EndpointBaseAsync.WithRequest<FileUploadRequest>.WithActionResult
    {
        private readonly ThunderServerContext _serverContext;

        //https://apiendpoints.ardalis.com/getting-started/patterns-used.html
        //USE THIS VIDEO FOR INFO https://www.youtube.com/watch?v=SDu0MA6TmuM
        public Create(ThunderServerContext serverContext)
        {
            this._serverContext = serverContext;
        }

        [HttpPost($"/api/{nameof(ThunderFile)}/Create")]
        [SwaggerOperation(
            Summary = "Creates a new Author",
            Description = "Creates a new Author",
            OperationId = "Author_Create",
            Tags = [$"{nameof(ThunderFile)}Endpoint"])]
        public override async Task<ActionResult> HandleAsync(FileUploadRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Check if files are present
                if (request.Files == null || request.Files.Count == 0)
                {
                    return BadRequest("No files were uploaded.");
                }

                // Directory to save uploaded files
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), request.PathToStoreFiles ?? "Uploads");

                ThunderFolder folder;

                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);

                    folder = new ThunderFolder
                    {
                        Name = uploadDirectory
                    };

                    await _serverContext.Folders.AddAsync(folder);
                    await _serverContext.SaveChangesAsync();
                }

                folder = await _serverContext.Folders.FirstAsync(x => x.Name.Equals(Directory.GetCurrentDirectory()), cancellationToken);

                var filePaths = new List<string>();

                foreach (var file in request.Files)
                {
                    if (file.Length > 0)
                    {
                        // Generate unique file name to prevent collisions
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadDirectory, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            var ownerGuid = Guid.Parse("B3C919D3-5410-4C0F-9D5A-3EA01A006EF5");
                            var parentFolderGuid = Guid.Parse("B3C919D3-5410-4C0F-9D5A-3EA01A006EF6");



                            ThunderFile thunderFile = new ThunderFile(fileName, file.ContentType, file.Length, filePath, ownerGuid, "", parentFolderGuid);

                            await _serverContext.Files.AddAsync(thunderFile, cancellationToken);
                            await _serverContext.SaveChangesAsync(cancellationToken);

                            await file.CopyToAsync(stream, cancellationToken);
                        }

                        filePaths.Add(filePath);
                    }
                }

                // Here you can process the uploaded files further if needed

                return Ok(filePaths);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
