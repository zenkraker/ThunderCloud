
using ThunderServer.API.Storage;
using ThunderServer.Infratructure.Storage;

namespace ThunderServer.API.Services
{
    public class FileStorer : BackgroundService
    {
        private readonly ThunderServerContext _serverContext;

        public FileStorer(ThunderServerContext serverContext) {
            this._serverContext = serverContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
        }
    }
}
