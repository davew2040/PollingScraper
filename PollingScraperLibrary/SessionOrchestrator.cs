using PollingScraperLibrary.Config;
using PollingScraperLibrary.QueryRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PollingScraperLibrary
{
    public class SessionOrchestrator
    {
        private IEnumerable<SourceDefinition> sources { get; set; }
        private IEnumerable<Task> activeTasks;
        private RootQueryRunner runner;
        private bool running = false;
        private CancellationTokenSource canceller;

        public SessionOrchestrator(RootConfig config, RootQueryRunner runner)
        {
            this.sources = SourcesBuilder.Build(config);
            this.runner = runner;
        }

        public void Start()
        {
            lock (this)
            {
                if (this.running)
                {
                    return;
                }

                this.canceller = new CancellationTokenSource();
                this.activeTasks = this.sources
                    .Select(s => Task.Run(async () =>
                        {
                            await this.runner.Run(s, this.canceller.Token);
                        }))
                    .ToList();
            }
        }

        public void Stop()
        {
            lock (this)
            {
                if (!this.running)
                {
                    return;
                }

                this.canceller.Cancel();
            }
        }
    }
}
