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
        private static string FILE_PATH = @"C:\temp\TennisWithMe\reports\";

        public void Configuration(IAppBuilder app)
        {
            Metric.Config.WithHttpEndpoint("http://localhost:12345/")
                .WithInternalMetrics()
                .WithReporting(config => config
                    .WithCSVReports(FILE_PATH, TimeSpan.FromSeconds(30))
                    .WithConsoleReport(TimeSpan.FromSeconds(30))
                    .WithTextFileReport(FILE_PATH + "metrics.txt", TimeSpan.FromSeconds(30))
                );

            ConfigureAuth(app);
        }
    }
}
