using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using JigsawService.Extensions;
using JigsawService.Templets;

namespace JigsawService
{
    internal static class StartUp
    {
        public static void ConfigureServces(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<IUser, Fake.User>();

            services.AddSingleton<IImageDecoder, JpegDecoder>();

            var config = context.Configuration;
            services.AddSingleton<IImages>(_ => new Images(

                decoder: _.GetRequiredService<IImageDecoder>(),
                limitations: (new Size(
                        config.Get("SizeLimits").Get("Width").Get("Min").ToInt(),
                        config.Get("SizeLimits").Get("Height").Get("Min").ToInt()
                    ),
                    new Size(
                        config.Get("SizeLimits").Get("Width").Get("Max").ToInt(),
                        config.Get("SizeLimits").Get("Height").Get("Max").ToInt()
                    )),
                prototype: (new Size(
                        config.Get("Prototype").Get("Size").Get("Width").ToInt(),
                        config.Get("Prototype").Get("Size").Get("Height").ToInt()
                    ),
                    new Size(
                        config.Get("Prototype").Get("Pieces").Get("Width").ToInt(),
                        config.Get("Prototype").Get("Pieces").Get("Height").ToInt()
                        )),
                logger: _.GetRequiredService<ILogger<Images>>()));

            //Func<string> generateId = Guid.NewGuid().ToString();
            Func<string> generateId = () => "42";

            services.AddSingleton<IStored<string, Image>>(_ => new MemoryStored<string, Image>(generateId));
            services.AddSingleton<IStored<string, int>>(_ => new MemoryStored<string, int>(generateId));

            services.AddSingleton<IRawTemplets, RawTemplets>();

            services.AddSingleton<ICoins, Fake.Coins>();

            services.AddHostedService<Worker>();
        }
    }
}
