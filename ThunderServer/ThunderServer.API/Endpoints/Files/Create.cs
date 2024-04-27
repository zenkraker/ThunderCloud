using Ardalis.ApiEndpoints;
using AutoMapper;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using ThunderServer.API.Configurations;
using ThunderServer.API.DTOs;
using ThunderServer.API.Services.Interfaces;
using ThunderServer.Models.Domain;

namespace ThunderServer.API.Endpoints.Files
{
	public class Create : EndpointBaseAsync.WithRequest<FileUploadRequest>.WithActionResult<FileUploadResponse>
    {
        private readonly StorageVolumesConfiguration _storageConfiguration;
        private readonly ILogger<Create> _logger;
        private readonly IFileManager _fileManager;
        private readonly IMapper _mapper;



        //https://apiendpoints.ardalis.com/getting-started/patterns-used.html
        //USE THIS VIDEO FOR INFO https://www.youtube.com/watch?v=SDu0MA6TmuM
        public Create(IOptionsMonitor<StorageVolumesConfiguration> options, ILogger<Create> logger, IFileManager fileManager, IMapper mapper)
        {
            this._storageConfiguration = options.CurrentValue;
            this._logger = logger;
            this._fileManager = fileManager;
            this._mapper = mapper;
        }

        [HttpPost($"/api/{nameof(ThunderFile)}/Create")]
        [SwaggerOperation(
            Summary = "Creates a new Author",
            Description = "Creates a new Author",
            OperationId = "Author_Create",
            Tags = [$"{nameof(ThunderFile)}Endpoint"])]
        public override async Task<ActionResult<FileUploadResponse>> HandleAsync(FileUploadRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Check if files are present
                if (request.Files == null || request.Files.Count == 0)
                {
                    return BadRequest("No files were uploaded.");
                }

                // Directory to save uploaded files
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), request.PathToStoreFiles ?? _storageConfiguration.UploadFolder);

                var results = await _fileManager.AddFileToFolder(request.Files, cancellationToken, uploadDirectory);

                return results.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while uploading file to server {fileName}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }        
    }
}
