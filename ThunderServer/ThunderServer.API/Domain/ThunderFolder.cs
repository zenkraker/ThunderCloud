namespace ThunderServer.API.Domain
{
    public class ThunderFolder
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentFolderId { get; set; } // Nullable for root folder
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<ThunderFile> Files { get; set; }
    }
}
