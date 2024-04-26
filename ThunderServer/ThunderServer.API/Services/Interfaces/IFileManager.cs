using FluentResults;
using ThunderServer.Models.Domain;

namespace ThunderServer.API.Services.Interfaces
{
    public interface IFileManager
    {
        Task<Result<List<ThunderFile>>> AddFileToFolder(List<IFormFile> formFiles, CancellationToken cancellationToken, string? uploadDirectory);
    }
}
