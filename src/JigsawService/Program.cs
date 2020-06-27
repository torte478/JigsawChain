using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JigsawService.Extensions;

namespace JigsawService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IUser, Fake.User>();
                    services.AddSingleton<
                        SixLabors.ImageSharp.Formats.IImageDecoder,
                        SixLabors.ImageSharp.Formats.Jpeg.JpegDecoder>();
                    services.AddSingleton<IImages>(_ => new Images(
                        decoder: _.GetRequiredService<SixLabors.ImageSharp.Formats.IImageDecoder>(),
                        limitations: (
                            (
                                hostContext.Configuration["SizeLimits:Width:Min"].ToInt(),
                                hostContext.Configuration["SizeLimits:Width:Max"].ToInt()
                            ),
                            (
                                hostContext.Configuration["SizeLimits:Height:Min"].ToInt(),
                                hostContext.Configuration["SizeLimits:Height:Max"].ToInt())
                            ),
                        logger: _.GetRequiredService<ILogger<Images>>()));
                    services.AddHostedService<Worker>();
                });
    }
}
