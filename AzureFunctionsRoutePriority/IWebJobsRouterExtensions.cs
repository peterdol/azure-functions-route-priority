using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace nrdkrmp.AzureFunctionsRoutePriority
{
    [ExcludeFromCodeCoverage]
    public static class IWebJobsRouterExtensions
    {
        public static IEnumerable<Route> GetRoutes(this IWebJobsRouter router)
        {
            var routes = typeof(WebJobsRouter)
                            .GetRuntimeFields()
                            .FirstOrDefault(f => f.Name == "_functionRoutes");

            if (routes == null)
                return new List<Route>();

            var routeCollection = (RouteCollection)routes.GetValue(router);
            return GetRoutes(routeCollection);
        }

        static IEnumerable<Route> GetRoutes(RouteCollection collection)
        {
            var routes = new List<Route>();

            for (var i = 0; i < collection.Count; i++)
            {
                if (collection[i] is RouteCollection nestedCollection)
                {
                    routes.AddRange(GetRoutes(nestedCollection));
                    continue;
                }
                routes.Add((Route)collection[i]);
            }

            return routes;
        }
    }
}
