using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                    services.AddTransient<IUser, Fake.User>();
                    services.AddTransient<
                        SixLabors.ImageSharp.Formats.IImageDecoder, 
                        SixLabors.ImageSharp.Formats.Jpeg.JpegDecoder>();
                    services.AddTransient<IImages, Images>();
                    services.AddHostedService<Worker>();
                });
    }
}
