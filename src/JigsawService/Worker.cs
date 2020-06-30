using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using JigsawService.Extensions;
using JigsawService.Images;
using JigsawService.Templets;
using JigsawService.Images.Pieces;

namespace JigsawService
{
    internal sealed class Worker : BackgroundService
    {
        private readonly IInput input;
        private readonly IImages images;
        private readonly IStored<string, Image> imageCache;
        private readonly IStored<string, TaskInfo> taskCache;
        private readonly IRawTemplets templets;

        private readonly ICoinService coinService;
        private readonly IImageService imageService;
        private readonly IPieceSerivce pieceService;

        private readonly ILogger<Worker> logger;

        public Worker(
                IInput input,
                IImages images,
                IStored<string, Image> imageCache,
                IStored<string, TaskInfo> taskCache,
                IRawTemplets templets,
                ICoinService coinService,
                IImageService imageService,
                IPieceSerivce pieceService,
                ILogger<Worker> logger)
        {
            this.input = input;
            this.images = images;
            this.imageCache = imageCache;
            this.taskCache = taskCache;
            this.templets = templets;

            this.coinService = coinService;
            this.imageService = imageService;
            this.pieceService = pieceService;

            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            input.UploadJigsaw += Input_UploadJigsaw;
            input.ChooseTemplet += Input_ChooseTemplet;
            input.ConfirmJigsaw += Input_ConfirmJigsaw;

            Task.Run(Fake);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private void Fake()
        {
            try
            {
                Task.Delay(100).Wait();

                (input as Fake.Input).RaiseUploadJigsawEvent(
                    @"d:\jigsawChain\images\input\1.jpg");

                (input as Fake.Input).RaiseChooseTempletEvent("42", 3, 1);

                (input as Fake.Input).RaiseConfirmJigsawEvent("42", true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        private void Input_UploadJigsaw(IRpcToken token, byte[] image)
        {
            image
            ._(images.Load)
            .Match(
                right: _ => ProcessJigsaw(token, _),
                left: _ => input.SendError(token, _));
        }

        private void ProcessJigsaw(IRpcToken token, Image image)
        {
            var stored = imageCache.Store(image);
            var templet = templets.Serialize();

            input.SendTemplet(token, stored, templet);
        }

        private void Input_ChooseTemplet(IRpcToken token, string id, string templet)
        {
            templet
            ._(templets.Deserialize)
            .Match(
                right: _ => ProcessTemplet(token, id, _),
                left: _ => input.SendError(token, _));
        }

        private void ProcessTemplet(IRpcToken token, string id, Templet templet)
        {
            var cost = coinService.CalculateCostAsync(templet);
            var preview = images.BuildPreview(imageCache.Read(id), templet);
            var stored = taskCache.Store(new TaskInfo
            {
                ImageId = id,
                Templet = templet,
                Edges = preview.edges,
                Cost = cost.Result
            });
            input.SendPreview(token, stored, preview.image, cost.Result);
        }

        private void Input_ConfirmJigsaw(IRpcToken token, string id, bool confirm)
        {
            var task = taskCache.Read(id);
            if (confirm)
            {
                var paid = coinService.TryPayJigsawCreationAsync(token, task.Cost);
                if (paid.Result.IsLeft)
                {
                    input.SendError(token, paid.Result.Left);
                    return;
                }

                var saved = imageService.SaveImageAsync(task.ImageId._(imageCache.Read));
                var scheduled = pieceService.SaveTaskAsync(token, task);
                if (saved.Result.IsLeft)
                {
                    input.SendError(token, paid.Result.Left);
                    return;
                }
                if (scheduled.Result.IsLeft)
                {
                    input.SendError(token, scheduled.Result.Left);
                    return;
                }

                input.SendConfirmation(token, scheduled.Result.Right);
            }

            imageCache.Remove(task.ImageId);
            taskCache.Remove(id);
        }
    }
}