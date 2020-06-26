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
                    var user = new Fake.User();
                    services.AddSingleton<IUser>(user);
                    services.AddSingleton<Fake.ISimulation>(new Fake.Simulation(user));
                    services.AddHostedService<Worker>();
                });
    }
}
