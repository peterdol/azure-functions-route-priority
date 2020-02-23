using System;
using Microsoft.AspNetCore.Routing;

namespace nrdkrmp.AzureFunctionsRoutePriority
{
    public class RoutePriorityOptions
    {
        public Comparison<Route> Comparison { get; set; }
    }
}