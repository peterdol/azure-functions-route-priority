using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Options;

namespace nrdkrmp.AzureFunctionsRoutePriority
{
    public class RoutePriorityExtensionConfigProvider : IExtensionConfigProvider
    {
        readonly IApplicationLifetime _applicationLifetime;
        readonly IWebJobsRouter _router;
        private readonly RoutePriorityOptions _options;

        public RoutePriorityExtensionConfigProvider(IApplicationLifetime applicationLifetime, IWebJobsRouter router, IOptionsMonitor<RoutePriorityOptions> optionsMonitor)
        {
            _applicationLifetime = applicationLifetime;
            _router = router;
            _options = optionsMonitor.CurrentValue;
            _applicationLifetime.ApplicationStarted.Register(() =>
            {
                ReorderRoutes();
            });
        }

        public void Initialize(ExtensionConfigContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
        }

        public void ReorderRoutes()
        {
            var routePrecedence = Comparer<Route>.Create(_options.Comparison);

            var functionRoutes = _router.GetFunctionRoutes()
                                        .ToEnumerable()
                                        .OrderBy(id => id, routePrecedence)
                                        .ToRouteCollection();

            var proxyRoutes = _router.GetProxyRoutes()
                                     .ToEnumerable()
                                     .OrderBy(id => id, routePrecedence)
                                     .ToRouteCollection();

            _router.ClearRoutes();
            _router.AddFunctionRoutes(functionRoutes, proxyRoutes);
        }
    }
}