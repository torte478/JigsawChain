using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using JigsawService.Extensions;
using JigsawService.Images;
using JigsawService.Images.Pieces;
using JigsawService.Templets;

namespace JigsawService
{
    internal static class StartUp
    {
        public static void ConfigureServces(
                                HostBuilderContext context, 
                                IServiceCollection services)
        {
            var config = context.Configuration;
            services.AddSingleton<IInput, Fake.Input>();

            services.AddSingleton<IImageDecoder, JpegDecoder>();

            services.AddSingleton(_ => new RawImages(

                decoder: _.GetRequiredService<IImageDecoder>(),

                limitations: (new Size(
                        config.Get("SizeLimits").Get("Width").Get("Min").ToInt(),
                        config.Get("SizeLimits").Get("Height").Get("Min").ToInt()
                    ),
                    new Size(
                        config.Get("SizeLimits").Get("Width").Get("Max").ToInt(),
                        config.Get("SizeLimits").Get("Height").Get("Max").ToInt()
                    )),

                logger: _.GetRequiredService<ILogger<RawImages>>()));

            services.AddSingleton(_ => Jigsaws.Create(

                size: new Size(
                        config.Get("Prototype").Get("Size").Get("Width").ToInt(),
                        config.Get("Prototype").Get("Size").Get("Height").ToInt()),

                pieces: 
                    new Size(
                        config.Get("Prototype").Get("Pieces").Get("Width").ToInt(),
                        config.Get("Prototype").Get("Pieces").Get("Height").ToInt()
                        ),

                buildPieces: SquarePiece.DecomposeImage));
                //buildPieces: new JigsawPieces(
                //        generateEdges: new RandomEdges().Generate,
                //        buildShapes: PieceShapes.Create)
                //    .Build));

            services.AddSingleton<IImages>(_ => new Images.Images(
                loadImage: _.GetRequiredService<RawImages>().Load,
                buildPreview: _.GetRequiredService<Jigsaws>().BuildPreview));

            //Func<string> generateId = Guid.NewGuid().ToString();
            Func<string> generateId = () => "42";

            services.AddSingleton<IStored<string, Image>>(
                _ => new MemoryStored<string, Image>(generateId));
            services.AddSingleton<IStored<string, TaskInfo>>(
                _ => new MemoryStored<string, TaskInfo>(generateId));

            services.AddSingleton<IRawTemplets, RawTemplets>();

            services.AddSingleton<ICoinService>(new Fake.CoinService(true));
            services.AddSingleton<IImageService>(new Fake.ImageService(true));
            services.AddSingleton<IPieceSerivce>(new Fake.PieceService(true));


            services.AddHostedService<Worker>();
        }
    }
}
