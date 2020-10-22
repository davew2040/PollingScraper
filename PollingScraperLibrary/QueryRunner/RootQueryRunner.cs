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
        private readonly HttpClient client;

        public RootQueryRunner(HttpClient client)
        {
            this.client = client;
        }

        public async Task Run(SourceDefinition source, CancellationToken token)
        {
            if (source.Source == SourceType.Amazon)
            {
                var amazon = new Amazon(this.client, source);
                await amazon.Run(token);
            }
            else if (source.Source == SourceType.NewEgg)
            {
                var newEgg = new NewEgg(this.client, source);
                await newEgg.Run(token);
            }
            else
            {
                throw new Exception($"Unrecognized source type {source.Source}");
            }
        }
    }
}
