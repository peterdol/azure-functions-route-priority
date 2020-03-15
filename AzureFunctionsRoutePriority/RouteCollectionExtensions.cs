using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace nrdkrmp.AzureFunctionsRoutePriority
{
    public static class RouteCollectionExtensions
    {
        public static RouteCollection ToRouteCollection(this IEnumerable<Route> routes)
        {
            var collection = new RouteCollection();

            foreach (var route in routes)
            {
                collection.Add(route);
            }

            return collection;
        }

        public static IEnumerable<Route> ToEnumerable(this RouteCollection collection)
        {
            var routes = new List<Route>();

            for (var i = 0; i < collection.Count; i++)
            {
                if (collection[i] is RouteCollection nestedCollection)
                {
                    routes.AddRange(nestedCollection.ToEnumerable());
                    continue;
                }

                routes.Add((Route)collection[i]);
            }

            return routes;
        }
    }
}
