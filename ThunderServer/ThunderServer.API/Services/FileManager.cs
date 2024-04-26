using AutoMapper;
using FluentResults;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using ThunderServer.API.Configurations;
using ThunderServer.API.Services.Interfaces;
using ThunderServer.Infratructure.Storage;
using ThunderServer.Models.Domain;

namespace ThunderServer.API.Services
{
    public class FileManager : IFileManager
    {

        private readonly StorageVolumesConfiguration _storageConfiguration;
        private readonly ILogger<FileManager> _logger;
        private readonly ThunderServerContext _serverContext;

        public FileManager(IOptionsMonitor<StorageVolumesConfiguration> options, ILogger<FileManager> logger, ThunderServerContext serverContext)
        {
            this._storageConfiguration = options.CurrentValue;
            this._logger = logger;
            this._serverContext = serverContext;
        }

        public async Task<Result<List<ThunderFile>>> AddFileToFolder(List<IFormFile> formFiles, CancellationToken cancellationToken, string? uploadDirectory)
        {
            if (string.IsNullOrEmpty(uploadDirectory))
            {
                uploadDirectory = _storageConfiguration.UploadFolder;
            }

            var creationResult = new List<ThunderFile>();

            foreach (var file in formFiles)
            {
                if (file.Length > 0)
                {
                    string fileName = file.FileName + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadDirectory, fileName);

                    var ownerGuid = Guid.Parse("B3C919D3-5410-4C0F-9D5A-3EA01A006EF5");

                    var parentFolder = this._serverContext.Folders.FirstOrDefault(x => x.Name == "uploads");

                    if (parentFolder is null)
                    {
                        parentFolder = ThunderFolder.Create("uploads", null);

                        await this._serverContext.Folders.AddAsync(parentFolder);

                        await this._serverContext.SaveChangesAsync();
                    }

                    //If the file already exists then it will rename it and set status to alreadyExisting
                    if (IsDuplicateFile(filePath))
                    {
                        var thunderFile = ThunderFile.CreateThunderFileAlreadyExisting(fileName, Path.GetExtension(file.FileName), file.Length, filePath, ownerGuid, "", parentFolder.Id);

                        creationResult.Add(thunderFile);
                    }
                    else
                    {
                        var thunderFile = ThunderFile.CreateThunderFile(fileName, Path.GetExtension(file.FileName), file.Length, filePath, ownerGuid, "", parentFolder.Id);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream, cancellationToken);
                        }

                        creationResult.Add(thunderFile);
                    }
                }
            }

            return Result.Ok(creationResult);
        }

        public static bool IsDuplicateFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                // File does not exist
                return false;
            }

            FileInfo fileInfo = new FileInfo(filePath);
            string fileName = fileInfo.Name;
            long fileSize = fileInfo.Length;
            string fileExtension = fileInfo.Extension;

            string checksum = CalculateChecksum(filePath);

            // Search for files with the same name, size, extension, and checksum
            string[] files = Directory.GetFiles(fileInfo.DirectoryName, $"{fileName}.{fileExtension}");
            foreach (string file in files)
            {
                FileInfo currentFile = new FileInfo(file);
                if (currentFile.Length == fileSize && CalculateChecksum(file) == checksum)
                {
                    return true; // Found a duplicate file
                }
            }

            return false; // No duplicate found
        }

        public static string CalculateChecksum(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] checksumBytes = md5.ComputeHash(stream);
                    return BitConverter.ToString(checksumBytes).Replace("-", "").ToLower();
                }
            }
        }

    }
}
