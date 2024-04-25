namespace ThunderServer.API.Domain
{
    public class RecycleBinItem
    {
        public Guid Id { get; set; }
        public ItemType ItemType { get; set; } // "File" or "Folder"
        public Guid ItemId { get; set; } // ID of the deleted item
        public DateTime DeletedAt { get; set; }

        // Navigation properties
        public virtual ThunderFolder ParentFolder { get; set; }
    }
}
