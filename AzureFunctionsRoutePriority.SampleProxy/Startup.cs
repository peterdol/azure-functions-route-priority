using System;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(nrdkrmp.AzureFunctionsRoutePriority.SampleProxy.Startup))]

namespace nrdkrmp.AzureFunctionsRoutePriority.SampleProxy
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.AddRoutePriority();
        }
    }
}