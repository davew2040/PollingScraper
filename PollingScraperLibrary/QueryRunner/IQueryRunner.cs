using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PollingScraperLibrary.QueryRunner
{
    public interface IQueryRunner
    {
        Task Run(CancellationToken token);
    }
}
