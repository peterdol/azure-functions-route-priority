# azure-functions-route-priority
An extension to add MVC .NET core like route priority to Azure Functions 

## Usage

```c#
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // add this to the Configure method in your Startup.cs
            builder.AddRoutePriority();
        }
```

