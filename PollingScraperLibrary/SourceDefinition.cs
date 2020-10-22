using System;
using System.Collections.Generic;
using System.Text;

namespace PollingScraperLibrary
{
    public class SourceDefinition
    {
        public string Title { get; set; }
        public string QueryUrl { get; set; }
        public SourceType Source { get; set; }
        public TimeSpan CycleDelay { get; set; }
    }
}
