using Microsoft.EntityFrameworkCore;
using ThunderServer.API.Domain;

namespace ThunderServer.API.Storage
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
