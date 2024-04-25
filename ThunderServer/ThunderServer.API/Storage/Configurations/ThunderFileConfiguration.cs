using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ThunderServer.API.Domain;

namespace ThunderServer.API.Storage.Configurations
{
    public class ThunderFileConfiguration : IEntityTypeConfiguration<ThunderFile>
    {
        public void Configure(EntityTypeBuilder<ThunderFile> builder)
        {
            builder.ToTable(nameof(ThunderFile)); // Table name in the database

            builder.HasKey(f => f.Id); // Primary key

            // Map entity properties to database columns
            builder.Property(f => f.FileName).IsRequired().HasMaxLength(255);
            builder.Property(f => f.FileType).IsRequired().HasMaxLength(50);
            builder.Property(f => f.FileSize).IsRequired();
            builder.Property(f => f.FilePath).IsRequired().HasMaxLength(255);
            builder.Property(f => f.OwnerId).IsRequired();
            builder.Property(f => f.UploadDate).IsRequired();
            builder.Property(f => f.LastModifiedDate).IsRequired();
            builder.Property(f => f.AccessPermissions).HasMaxLength(50);
            builder.Property(f => f.Metadata).HasMaxLength(255);
            builder.Property(f => f.Checksum).HasMaxLength(255);
            builder.Property(f => f.IsEncrypted).IsRequired();
            builder.Property(f => f.IsShared).IsRequired();
            builder.Property(f => f.Version).IsRequired();
            builder.Property(f => f.ParentFolderId).IsRequired();

            // You can define any additional configurations here
        }
    }
}
