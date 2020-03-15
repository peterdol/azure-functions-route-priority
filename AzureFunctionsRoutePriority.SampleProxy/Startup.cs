using System.Diagnostics;
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.AspNetCore.Routing;
using nrdkrmp.AzureFunctionsRoutePriority;

[assembly: WebJobsStartup(typeof(nrdkrmp.AzureFunctionsRoutePriority.SampleProxy.Startup))]

namespace nrdkrmp.AzureFunctionsRoutePriority.SampleProxy
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            builder.AddRoutePriority();
        }
    }
}