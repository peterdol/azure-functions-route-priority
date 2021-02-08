using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

namespace nrdkrmp.AzureFunctionsRoutePriority
{
    public static class FunctionsHostBuilderExtensions
    {
        public static IFunctionsHostBuilder AddRoutePriority(this IFunctionsHostBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            var webBuilder = builder.Services.AddWebJobs(x => { return; });
            webBuilder.AddRoutePriority();

            return builder;
        }

        public static IFunctionsHostBuilder AddRoutePriority(this IFunctionsHostBuilder builder,
    Action<RoutePriorityOptions> configure)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (configure is null)
                throw new ArgumentNullException(nameof(configure));

            builder.AddRoutePriority();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
