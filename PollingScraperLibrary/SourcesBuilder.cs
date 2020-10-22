using PollingScraperLibrary.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PollingScraperLibrary
{
    public class SourcesBuilder
    {
        public static IEnumerable<SourceDefinition> Build(RootConfig config)
        {
            IEnumerable<SourceDefinition> definitions = config.Sources.Select(source =>
            {
                var definition = new SourceDefinition()
                {
                    QueryUrl = source.QueryUrl,
                    Title = source.Title
                };

                definition.CycleDelay = TimeSpan.FromSeconds(2);
                definition.Source = DetermineUrlType(source.QueryUrl);

                return definition;
            });

            return definitions.ToList();
        }

        private static SourceType DetermineUrlType(string url)
        {
            var uri = new Uri(url);
            var host = uri.Host.ToLower();

            if (host.Contains("newegg.com"))
            {
                return SourceType.NewEgg;
            }
            else if (host.Contains("amazon.com"))
            {
                return SourceType.Amazon;
            }

            return SourceType.Unknown;
        }
    }
}
