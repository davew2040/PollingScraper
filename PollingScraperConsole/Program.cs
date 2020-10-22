using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PollingScraperLibrary;
using PollingScraperLibrary.Config;
using PollingScraperLibrary.QueryRunner;
using System;
using System.IO;
using System.Net.Http;

namespace PollingScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = GetConfiguration(args);

            var rootConfig = new RootConfig(config);

            var serviceProvider = new ServiceCollection()
                .AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.SetMinimumLevel(LogLevel.Information);
                })
                .AddSingleton<IConfiguration>(config)
                .AddSingleton(rootConfig)
                .AddSingleton<SessionOrchestrator>()
                .AddSingleton<HttpClient>()
                .AddSingleton<RootQueryRunner>()
                .BuildServiceProvider();

            var orc = serviceProvider.GetService<SessionOrchestrator>();
            var logger = serviceProvider.GetService<ILogger<Program>>();

            logger.LogError("test");
            logger.LogDebug("Starting application");

            orc.Start();

            while (Console.ReadKey().Key != ConsoleKey.X)
            {
                // spin
            }

            logger.LogDebug("All done!");
        }

        private static IConfiguration GetConfiguration(string[] args)
        {
            if (args.Length == 2)
            {
                var jsonStream = new FileStream(args[1], FileMode.Open);
                return new ConfigurationBuilder()
                    .AddJsonStream(jsonStream)
                    .Build();
            }
            else
            {
                return new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
            }

        }
    }
}
 