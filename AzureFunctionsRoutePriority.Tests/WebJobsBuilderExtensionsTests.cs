using System.IO.Compression;
using System.Runtime.CompilerServices;
using Xunit;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Moq;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Config;
using NSubstitute;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace nrdkrmp.AzureFunctionsRoutePriority.Tests
{
    public class WebJobsBuilderExtensionsTests
    {
        [Fact]
        public void CanConfigureDefaultRoutePriority()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization());

            var constraintResolver = new Mock<IInlineConstraintResolver>();
            var handler = new Mock<IWebJobsRouteHandler>();
            IWebJobsRouter router = new WebJobsRouter(constraintResolver.Object);

            var builder = router.CreateBuilder(handler.Object, "api");
            builder.MapFunctionRoute("testfunction1", "test/{token}", "testfunction1");
            builder.MapFunctionRoute("testfunction2", "test/x", "testfunction2");
            router.AddFunctionRoutes(builder.Build(), null);

            var services = new ServiceCollection();
            services.AddSingleton(Substitute.For<IApplicationLifetime>());
            services.AddSingleton(router);

            var mockBuilder = new Mock<IWebJobsBuilder>();
            mockBuilder.SetupGet(x => x.Services).Returns(services).Verifiable();

            // act
            var actual = mockBuilder.Object.AddRoutePriority();
            var routePriorityExtension = actual.Services.BuildServiceProvider().GetService<IExtensionConfigProvider>() as RoutePriorityExtensionConfigProvider;
            routePriorityExtension.ReorderRoutes();

            // assert
            var routes = router.GetRoutes();
            Assert.Equal("testfunction2", routes.First().Name);
        }

        [Fact]
        public void CanOverrideDefaultRoutePriority()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization());

            var constraintResolver = new Mock<IInlineConstraintResolver>();
            var handler = new Mock<IWebJobsRouteHandler>();
            IWebJobsRouter router = new WebJobsRouter(constraintResolver.Object);

            var builder = router.CreateBuilder(handler.Object, "api");
            builder.MapFunctionRoute("testfunction1", "test/{token}", "testfunction1");
            builder.MapFunctionRoute("testfunction2", "test/xxxxxxxxxx", "testfunction2");
            router.AddFunctionRoutes(builder.Build(), null);

            var services = new ServiceCollection();
            services.AddSingleton(Substitute.For<IApplicationLifetime>());
            services.AddSingleton(router);

            var mockBuilder = new Mock<IWebJobsBuilder>();
            mockBuilder.SetupGet(x => x.Services).Returns(services).Verifiable();

            // act
            var actual = mockBuilder.Object.AddRoutePriority(
                o => o.Comparison = (Route x, Route y) => x.RouteTemplate.Length.CompareTo(y.RouteTemplate.Length)
            );
            var routePriorityExtension = actual.Services.BuildServiceProvider().GetService<IExtensionConfigProvider>() as RoutePriorityExtensionConfigProvider;
            routePriorityExtension.ReorderRoutes();

            // assert
            var routes = router.GetRoutes();
            Assert.Equal("testfunction2", routes.First().Name);
        }

        [Fact]
        public void DoesNotClearProxyRoutes()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization());

            var constraintResolver = new Mock<IInlineConstraintResolver>();
            var handler = new Mock<IWebJobsRouteHandler>();
            IWebJobsRouter router = new WebJobsRouter(constraintResolver.Object);

            var builder = router.CreateBuilder(handler.Object, "api");
            builder.MapFunctionRoute("testfunction1", "test/{token}", "testfunction1");
            builder.MapFunctionRoute("testfunction2", "test/xxxxxxxxxx", "testfunction2");

            var proxyBuilder = router.CreateBuilder(handler.Object, "proxy");
            proxyBuilder.MapFunctionRoute("proxy1", "dummy", null);

            router.AddFunctionRoutes(builder.Build(), proxyBuilder.Build());

            var services = new ServiceCollection();
            services.AddSingleton(Substitute.For<IApplicationLifetime>());
            services.AddSingleton(router);

            var mockBuilder = new Mock<IWebJobsBuilder>();
            mockBuilder.SetupGet(x => x.Services).Returns(services).Verifiable();

            // act
            var actual = mockBuilder.Object.AddRoutePriority(
                o => o.Comparison = (Route x, Route y) => x.RouteTemplate.Length.CompareTo(y.RouteTemplate.Length)
            );
            var routePriorityExtension = actual.Services.BuildServiceProvider().GetService<IExtensionConfigProvider>() as RoutePriorityExtensionConfigProvider;
            routePriorityExtension.ReorderRoutes();

            // assert
            var routes = router.GetProxyRoutes();
            Assert.Equal(1, routes.Count);
        }
    }
}
