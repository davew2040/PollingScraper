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
                .AddSingleton<BrowserRunner>()
                .BuildServiceProvider();

            var orc = serviceProvider.GetService<SessionOrchestrator>();
            var logger = serviceProvider.GetService<ILogger<Program>>();

            logger.LogDebug("Starting application");

            orc.Start();

            StartConsoleLoop(serviceProvider);

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

        private static void StartConsoleLoop(ServiceProvider serviceProvider)
        {
            var orc = serviceProvider.GetService<SessionOrchestrator>();
            var runner = serviceProvider.GetService<BrowserRunner>();

            var command = Console.ReadLine().ToLower().Trim();
            bool done = false;

            while (!done)
            {
                if (command == "exit")
                {
                    done = true;
                }
                else if (command == "test")
                {
                    runner.Run("www.google.com");
                }
                else if (command == "stop")
                {
                    orc.Stop();
                }
                else if (command == "start")
                {
                    orc.Start();
                }
            }

        }
    }
}
 