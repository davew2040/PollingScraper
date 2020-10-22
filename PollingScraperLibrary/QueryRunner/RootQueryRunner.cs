using Microsoft.Extensions.Logging;
using PollingScraperLibrary.QueryRunner.SpecificRunners;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PollingScraperLibrary.QueryRunner
{
    public class RootQueryRunner
    {
        private readonly HttpClient _client;
        private readonly BrowserRunner _runner;
        private readonly ILogger<RootQueryRunner> _logger;
        private readonly SessionOrchestrator _orch;

        public RootQueryRunner(HttpClient client, ILogger<RootQueryRunner> logger, BrowserRunner runner)
        {
            _client = client;
            _runner = runner;
            _logger = logger;
        }

        public async Task Run(SourceDefinition source, CancellationToken token)
        {
            if (source.Source == SourceType.Amazon)
            {
                var amazon = new Amazon(_client, _logger, _runner, source);
                await amazon.Run(token);
            }
            else if (source.Source == SourceType.NewEgg)
            {
                var newEgg = new NewEgg(_client, source);
                await newEgg.Run(token);
            }
            else
            {
                throw new Exception($"Unrecognized source type {source.Source}");
            }
        }
    }
}
