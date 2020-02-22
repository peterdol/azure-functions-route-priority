using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.WebJobs;

namespace nrdkrmp.AzureFunctionsRoutePriority
{
    public static class WebJobsBuilderExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IWebJobsBuilder AddRoutePriority(this IWebJobsBuilder builder)
        {
            if (builder is null)
                throw new System.ArgumentNullException(nameof(builder));

            builder.AddExtension<RoutePriorityExtensionConfigProvider>();

            return builder;
        }
    }
}
