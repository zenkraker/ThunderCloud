namespace ThunderServer.Models.Domain
{
    public class ThunderFolder
    {
        private ThunderFolder(string name, Guid? parentFolderId, DateTime utcNow)
        {
            Name = name;
            ParentFolderId = parentFolderId;
            CreatedAt = utcNow;
            IsRootFolder = parentFolderId.HasValue;
        }

        public Guid Id { get; init; }
        public string Name { get; init; }
        public Guid? ParentFolderId { get; init; } // Nullable for root folder
        public DateTime CreatedAt { get; init; }
        public bool IsRootFolder { get; init; }

        public static ThunderFolder Create(string name, Guid? parentFolderId)
        {
            return new ThunderFolder(name, parentFolderId, DateTime.UtcNow);
        }

        // Navigation properties
        public virtual ICollection<ThunderFile> Files { get; init; }
    }
}
