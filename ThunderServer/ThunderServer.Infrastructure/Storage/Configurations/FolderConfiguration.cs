using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThunderServer.Models.Domain;

namespace ThunderServer.API.Storage.Configurations
{
    public class FolderConfiguration : IEntityTypeConfiguration<ThunderFolder>
    {
        public void Configure(EntityTypeBuilder<ThunderFolder> builder)
        {
            builder.ToTable(nameof(ThunderFolder)); // Table name in the database

            builder.HasKey(f => f.Id); // Primary key

            // Map entity properties to database columns
            builder.Property(f => f.Name).IsRequired().HasMaxLength(255);
            builder.Property(f => f.ParentFolderId);
            builder.Property(f => f.CreatedAt).ValueGeneratedOnAdd();

            builder.HasMany(x => x.Files).WithOne(x => x.ParentFolder).HasForeignKey(x => x.ParentFolderId);
        }
    }
}
