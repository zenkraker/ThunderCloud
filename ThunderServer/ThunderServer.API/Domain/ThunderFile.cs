namespace ThunderServer.API.Domain
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

        // Constructor
        //If ParentFolderId is null it means that it is on the root folder
        public ThunderFile(string fileName, string fileType, long fileSize, string filePath, Guid ownerId, string accessPermissions, Guid? parentFolderId = null)
        {
            Id = Guid.NewGuid();
            FileName = fileName;
            FileType = fileType;
            FileSize = fileSize;
            FilePath = filePath;
            OwnerId = ownerId;
            UploadDate = DateTime.UtcNow;
            LastModifiedDate = UploadDate;
            AccessPermissions = accessPermissions;
            Metadata = string.Empty;
            Checksum = string.Empty;
            IsEncrypted = false;
            IsShared = false;
            Version = 1;
            ParentFolderId = parentFolderId;
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

        // Navigation properties
        public Guid? ParentFolderId { get; set; }
        public virtual ThunderFolder ParentFolder { get; set; }
    }
}
