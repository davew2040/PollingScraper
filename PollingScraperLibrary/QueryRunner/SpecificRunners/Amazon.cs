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
        private readonly HttpClient Client;
        private readonly SourceDefinition Source;

        public Amazon(HttpClient client, SourceDefinition source) => (Client, Source) = (client, source);

        public async Task Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var response = await this.Client.GetAsync(this.Source.QueryUrl, token);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("Auto Notify"))
                    {
                        Console.WriteLine("Found Auto Notify");
                    }
                    else if (content.Contains("Add to Cart"))
                    {
                        Console.WriteLine("Found Add to Cart");
                    }
                }

                await Task.Delay(this.Source.CycleDelay);
            }
        }
    }
}
