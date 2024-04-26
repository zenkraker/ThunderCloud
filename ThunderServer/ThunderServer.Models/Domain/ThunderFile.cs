namespace ThunderServer.Models.Domain
{
    public class ThunderFile
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; }
        public string FileType { get; private set; }
        public long FileSize { get; private set; }
        public string FilePath { get; private set; }
        public Guid OwnerId { get; private set; }
        public DateTime UploadDate { get; private set; }
        public DateTime LastModifiedDate { get; private set; }
        public string AccessPermissions { get; private set; }
        public string Metadata { get; private set; }
        public string Checksum { get; private set; }
        public bool IsEncrypted { get; private set; }
        public bool IsShared { get; private set; }
        public int Version { get; private set; }

        public ThunderFileStatus FileStatus { get; private set; }

        // Constructor
        //If ParentFolderId is null it means that it is on the root folder

        public static ThunderFile CreateThunderFile(string fileName, string fileType, long fileSize, string filePath, Guid ownerId, string accessPermissions, Guid? parentFolderId = null)
        {
            return new ThunderFile(Guid.NewGuid(), fileName, fileType, fileSize, filePath, ownerId, accessPermissions, ThunderFileStatus.Created, parentFolderId);
        }

        public static ThunderFile CreateThunderFileAlreadyExisting(string fileName, string fileType, long fileSize, string filePath, Guid ownerId, string accessPermissions, Guid? parentFolderId = null)
        {
            return new ThunderFile(Guid.NewGuid(), SaveFileWithUniqueName(filePath), fileType, fileSize, filePath, ownerId, accessPermissions, ThunderFileStatus.AlreadyExisting, parentFolderId);
        }

        private ThunderFile(Guid guid, string fileName, string fileType, long fileSize, string filePath, Guid ownerId, string accessPermissions, ThunderFileStatus thunderFileStatus, Guid? parentFolderId = null)
        {
            Id = guid;
            FileName = fileName;
            FileType = fileType;
            FileSize = fileSize;
            FilePath = filePath;
            OwnerId = ownerId;
            UploadDate = DateTime.UtcNow;
            AccessPermissions = accessPermissions;
            Metadata = string.Empty;
            Checksum = string.Empty;
            IsEncrypted = false;
            IsShared = false;
            Version = 1;
            ParentFolderId = parentFolderId;
            LastModifiedDate = DateTime.UtcNow;
            FileStatus = thunderFileStatus;
        }

        // Method to update file properties
        public void UpdateFile(string fileName, string fileType, long fileSize, string filePath, string accessPermissions, string metadata, bool isEncrypted, bool isShared)
        {
            FileName = fileName;
            FileType = fileType;
            FileSize = fileSize;
            FilePath = filePath;
            LastModifiedDate = DateTime.UtcNow;
            AccessPermissions = accessPermissions;
            Metadata = metadata;
            IsEncrypted = isEncrypted;
            IsShared = isShared;
            Version++; // Increment version with each update
        }

        private static string SaveFileWithUniqueName(string filePath)
        {
            if (!File.Exists(filePath))
            {
                // File does not exist, no need to rename
                return filePath;
            }

            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string fileExtension = Path.GetExtension(filePath);

            int counter = 1;
            string newFilePath = Path.Combine(directory, $"{fileName} ({counter}){fileExtension}");

            while (File.Exists(newFilePath))
            {
                counter++;
                newFilePath = Path.Combine(directory, $"{fileName} ({counter}){fileExtension}");
            }

            File.Move(filePath, newFilePath);

            Console.WriteLine($"File renamed to: {Path.GetFileName(newFilePath)}");

            return newFilePath;
        }


        // Navigation properties
        public Guid? ParentFolderId { get; set; }
        public virtual ThunderFolder ParentFolder { get; set; }
    }
}
