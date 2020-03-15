using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace nrdkrmp.AzureFunctionsRoutePriority
{
    public static class WebJobsBuilderExtensions
    {
        public static IWebJobsBuilder AddRoutePriority(this IWebJobsBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.AddExtension<RoutePriorityExtensionConfigProvider>()
                   .ConfigureOptions<RoutePriorityOptions>((config, options) =>
                    {
                        config.Bind(options);
                        options.Comparison = DefaultRouteComparison.LiteralsFirst;
                    });

            return builder;
        }

        public static IWebJobsBuilder AddRoutePriority(this IWebJobsBuilder builder,
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
