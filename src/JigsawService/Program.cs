using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Hosting;
using JigsawService.Extensions;

[assembly: InternalsVisibleTo("JigsawService.Tests")]

namespace JigsawService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception ex)
            {
                ex.SaveTo(@"d:\jigsawChain\images\output\last_exception.txt");
                Console.WriteLine($"Error: {ex.Message})");
                return;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(StartUp.ConfigureServces);
    }
}
