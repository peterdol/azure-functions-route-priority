# azure-functions-route-priority
When a default function app starts up, the functions runtime locates all functions and then registers the routes for each of them. After this, routes are simply matched in the order that they are registered. This nuget package adds MVC like route priority to the Azure functions runtime:  

* A literal wins over a parameter in precedence.
* For literals with different values (case insensitive) we choose the lexical order
* For parameters with different numbers of constraints, the one with more wins

The package is based upon this blog post https://briandunnington.github.io/azure_functions_route_priority.

## Usage

```c#
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // add this to the Configure method in your Startup.cs
            builder.AddRoutePriority();
        }
```

