using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JigsawService
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IUser user;
        private readonly Fake.ISimulation simulation;

        public Worker(ILogger<Worker> logger, IUser user, Fake.ISimulation simulation)
        {
            _logger = logger;
            this.user = user;
            this.simulation = simulation;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            user.UploadJigsaw += User_UploadJigsaw;

            simulation.UserUploadJigsaw(@"d:\jigsawChain\images\1.jpg");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private void User_UploadJigsaw(IRpcToken id, byte[] image)
        {
            _logger.LogInformation($"RPC: {image.Length}");
        }
    }
}
