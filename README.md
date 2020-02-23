# azure-functions-route-priority
When a default function app starts up, the functions runtime locates all functions and then registers the routes for each of them. After this, routes are simply matched in the order that they are registered. This nuget package adds MVC like route priority to the Azure functions runtime:  

* A literal wins over a parameter in precedence.
* For literals with different values (case insensitive) lexical order applies
* For parameters with different numbers of constraints, the one with more constraints wins

The package is inspired by this blog post https://briandunnington.github.io/azure_functions_route_priority.

## Usage

To use the default route priority configure the extension like this in your startup.cs:
```c#
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // add this to the Configure method in your Startup.cs
            builder.AddRoutePriority();
        }
```

You can also use your own Route comparison by passing a custom Route Comparison function:
```c#
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // add this to the Configure method in your Startup.cs. Write your own Route comparison function.
            builder.AddRoutePriority(
                o => o.Comparison = (Route x, Route y) => x.RouteTemplate.Length.CompareTo(y.RouteTemplate.Length)
            );
        }
```

