using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ThunderServer.Infrastructure.Storage.Repositories
{
    public class ThunderFileRepository<ThunderFile> : RepositoryBase<ThunderFile> where ThunderFile : class
    {
        public ThunderFileRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
