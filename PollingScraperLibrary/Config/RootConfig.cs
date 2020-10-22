using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PollingScraperLibrary.Config
{
    public class RootConfig
    {
        public IEnumerable<SourceConfig> Sources { get; set; }
        public string BrowserPath { get; set; }

        public RootConfig(IConfiguration config)
        { 
            var sources = new List<SourceConfig>();

            this.BrowserPath = config.GetSection("browserPath").Value;

            var configSources = config.GetSection("sources").GetChildren();

            foreach (var configSource in configSources)
            {
                var title = configSource.GetSection("Title");
                var url = configSource.GetSection("QueryUrl");

                sources.Add(new SourceConfig(title.Value, url.Value));
            }

            this.Sources = sources;
        }
    }

}
