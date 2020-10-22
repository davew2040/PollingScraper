using System;
using System.Collections.Generic;
using System.Text;

namespace PollingScraperLibrary.Config
{
    public class SourceConfig
    {
        public string Title { get; }
        public string QueryUrl { get; }

        public SourceConfig(string title, string queryUrl) => (Title, QueryUrl) = (title, queryUrl);
    }
}
