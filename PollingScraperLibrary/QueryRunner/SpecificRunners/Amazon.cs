using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PollingScraperLibrary.QueryRunner.SpecificRunners
{
    public class Amazon : IQueryRunner
    {
        private readonly HttpClient _client;
        private readonly SourceDefinition _source;
        private readonly ILogger _logger;
        private readonly BrowserRunner _runner;

        public Amazon(HttpClient client, ILogger logger, BrowserRunner runner, SourceDefinition source) => (_client, _logger, _runner, _source) = (client, logger, runner, source);

        public async Task Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var response = await this._client.GetAsync(this._source.QueryUrl, token);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (content.Contains("See All Buying Options"))
                    {
                        this._logger.LogInformation($"Amazon - found {this._source.Title}");
                        _runner.Run(_source.QueryUrl);
                    }
                }

                await Task.Delay(this._source.CycleDelay);
            }
        }
    }
}
