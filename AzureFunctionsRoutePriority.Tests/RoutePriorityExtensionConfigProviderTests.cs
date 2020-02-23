using System.Linq;
using Xunit;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Moq;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Microsoft.AspNetCore.Hosting;

namespace nrdkrmp.AzureFunctionsRoutePriority.Tests
{
    public class RoutePriorityExtensionConfigProviderTests
    {
        [Fact]
        public void CanOrderRoutes()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization());

            var constraintResolver = new Mock<IInlineConstraintResolver>();
            constraintResolver.Setup(x => x.ResolveConstraint("int")).Returns(Substitute.For<IRouteConstraint>());
            var handler = new Mock<IWebJobsRouteHandler>();
            IWebJobsRouter router = new WebJobsRouter(constraintResolver.Object);

            var builder = router.CreateBuilder(handler.Object, "api");
            builder.MapFunctionRoute("testfunction1", "test/{token}", "testfunction1");
            builder.MapFunctionRoute("testfunction2", "test/{token:int}", "testfunction2");
            builder.MapFunctionRoute("testfunction3", "test/abc", "testfunction3");
            router.AddFunctionRoutes(builder.Build(), null);

            var provider = new RoutePriorityExtensionConfigProvider(Substitute.For<IApplicationLifetime>(), router);
            provider.ReorderRoutes();

            var routes = router.GetRoutes().ToList();
            Assert.Equal("testfunction3", routes[0].Name);
            Assert.Equal("testfunction2", routes[1].Name);
            Assert.Equal("testfunction1", routes[2].Name);
        }
    }
}
