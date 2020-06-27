using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using JigsawService.Extensions;
using JigsawService.Templets;

namespace JigsawService
{
    internal sealed class Worker : BackgroundService
    {
        private readonly IUser user;
        private readonly IImages images;
        private readonly IStored<string, Image> cache;
        private readonly IRawTemplets templets;
        private readonly ICoins coins;
        private readonly IStored<string, int> costs;
        private readonly ILogger<Worker> logger;

        public Worker(
                IUser user, 
                IImages images, 
                IStored<string, Image> cache, 
                IRawTemplets templets,
                ICoins coins,
                IStored<string, int> costs,
                ILogger<Worker> logger)
        {
            this.user = user;
            this.images = images;
            this.cache = cache;
            this.templets = templets;
            this.coins = coins;
            this.costs = costs;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            user.UploadJigsaw += User_UploadJigsaw;
            user.ChooseTemplet += User_ChooseTemplet;

            Task.Run(Fake);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private void Fake()
        {
            try
            {
                Task.Delay(100).Wait();
                (user as Fake.User).RaiseUploadJigsawEvent(@"d:\jigsawChain\images\input\3.jpg");
                (user as Fake.User).RaiseChooseTempletEvent("42", 0, 0);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        private void User_UploadJigsaw(IRpcToken token, byte[] image)
        {
            image
            ._(images.Load)
            .Match(
                right: _ => ProcessJigsaw(token, _),
                left: _ => user.SendError(token, _));
        }

        private void ProcessJigsaw(IRpcToken token, Image image)
        {
            var stored = cache.Store(image);
            var templet = templets.Serialize();

            user.SendTemplet(token, stored, templet);
        }

        private void User_ChooseTemplet(IRpcToken token, string id, string templet)
        {
            templet
            ._(templets.Deserialize)
            .Match(
                right: _ => ProcessTemplet(token, id, _),
                left: _ => user.SendError(token, _));
        }

        private void ProcessTemplet(IRpcToken token, string id, Templet templet)
        {
            var cost = coins.CalculateCostAsync(templet);
            var preview = images.BuildPreview(cache.Extract(id), templet);
            var stored = cache.Store(preview);
            costs.Store(stored, cost.Result);
            user.SendPreview(token, stored, preview, cost.Result);
        }
    }
}