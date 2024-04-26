namespace ThunderServer.API.DTOs
{
    public sealed record ThunderFileDto
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
    }
}