using Microsoft.EntityFrameworkCore;
using ThunderServer.Models.Domain;

namespace ThunderServer.Infratructure.Storage
{
    public class ThunderServerContext : DbContext
    {
        public DbSet<ThunderFile> Files { get; set; }
        public DbSet<ThunderFolder> Folders { get; set; }

        public ThunderServerContext(DbContextOptions options) : base(options)
        {
        }

        protected ThunderServerContext()
        {

        }
    }
}
