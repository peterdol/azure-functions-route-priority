using Xunit;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Moq;
using Microsoft.AspNetCore.Routing;

namespace nrdkrmp.AzureFunctionsRoutePriority.Tests
{
    public class IWebJobsRouterExtensionsTests
    {
        [Fact]
        public void CanGetRoutes()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization());

            var constraintResolver = new Mock<IInlineConstraintResolver>();
            var handler = new Mock<IWebJobsRouteHandler>();
            IWebJobsRouter router = new WebJobsRouter(constraintResolver.Object);

            var builder = router.CreateBuilder(handler.Object, "api");
            builder.MapFunctionRoute("testfunction", "test/{token}", "testfunction");
            router.AddFunctionRoutes(builder.Build(), null);

            var routes = router.GetRoutes();

            Assert.Single(routes);
        }
    }
}
