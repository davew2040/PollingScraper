using PollingScraperLibrary.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PollingScraperLibrary
{
    public class BrowserRunner
    {
        private readonly string _browserPath;

        public BrowserRunner(RootConfig config)
        {
            this._browserPath = config.BrowserPath;
        }

        public void Run(string url)
        {
            Process.Start(_browserPath, url);
        }
    }
}
