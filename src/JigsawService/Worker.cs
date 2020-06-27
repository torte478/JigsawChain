using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using JigsawService.Extensions;

namespace JigsawService
{
    internal sealed class Worker : BackgroundService
    {
        private readonly IUser user;
        private readonly IImages images;
        private readonly IStored<string, IImage> cache;
        private readonly IRawTemplets templets;
        private readonly ILogger<Worker> logger;

        public Worker(
                IUser user, 
                IImages images, 
                IStored<string, IImage> cache, 
                IRawTemplets templets, 
                ILogger<Worker> logger)
        {
            this.user = user;
            this.images = images;
            this.cache = cache;
            this.templets = templets;
            this.logger = logger;
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
            var target = image._(images.Load);
            if (target.IsLeft)
            {
                target.Left._(_ => user.SendError(id, _));
                return;
            }

            var stored = cache.Store(target.Right);
            var templet = templets.Serialize();

            logger.LogDebug($"{stored} {templet}");
        }
    }
}