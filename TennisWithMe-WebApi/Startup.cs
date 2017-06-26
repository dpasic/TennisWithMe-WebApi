using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Metrics;

[assembly: OwinStartup(typeof(TennisWithMe_WebApi.Startup))]

namespace TennisWithMe_WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Metric.Config.WithHttpEndpoint("http://localhost:12345/")
                .WithInternalMetrics()
                .WithReporting(config => config
                    .WithCSVReports(@"c:\temp\reports\", TimeSpan.FromSeconds(30))
                    .WithConsoleReport(TimeSpan.FromSeconds(30))
                    .WithTextFileReport(@"C:\temp\reports\metrics.txt", TimeSpan.FromSeconds(30))
                );

            ConfigureAuth(app);
        }
    }
}
