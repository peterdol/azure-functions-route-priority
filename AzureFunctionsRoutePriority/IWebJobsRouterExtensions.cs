using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace nrdkrmp.AzureFunctionsRoutePriority
{
    public static class IWebJobsRouterExtensions
    {
        public static RouteCollection GetFunctionRoutes(this IWebJobsRouter router) =>
             GetRouteCollection(router, "_functionRoutes");

        public static RouteCollection GetProxyRoutes(this IWebJobsRouter router) =>
             GetRouteCollection(router, "_proxyRoutes");

        private static RouteCollection GetRouteCollection(IWebJobsRouter router, string fieldName)
        {
            var routes = typeof(WebJobsRouter)
                .GetRuntimeFields()
                .FirstOrDefault(f => f.Name == fieldName);

            if (routes == null)
                return new RouteCollection();

            return (RouteCollection)routes.GetValue(router);
        }
    }
}
