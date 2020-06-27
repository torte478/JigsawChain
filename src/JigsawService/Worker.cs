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

        public Worker(IUser user, ILogger<Worker> logger)
        {
            _logger = logger;
            this.user = user;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            user.UploadJigsaw += User_UploadJigsaw;

            Task.Run(Fake);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private void Fake()
        {
            Task.Delay(100).Wait();
            (user as Fake.User).RaiseUploadJigsawEvent(@"d:\jigsawChain\images\1.jpg");
        }

        private void User_UploadJigsaw(IRpcToken id, byte[] image)
        {
            _logger.LogInformation($"RPC: {image.Length}");
        }
    }
}
