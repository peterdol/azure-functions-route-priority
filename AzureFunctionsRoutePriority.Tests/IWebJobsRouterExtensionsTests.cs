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
            // arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization());

            var constraintResolver = new Mock<IInlineConstraintResolver>();
            var handler = new Mock<IWebJobsRouteHandler>();
            var router = new WebJobsRouter(constraintResolver.Object);

            var builder = router.CreateBuilder(handler.Object, "api");
            builder.MapFunctionRoute("testfunction", "test/{token}", "testfunction");
            router.AddFunctionRoutes(builder.Build(), null);

            // act
            var routes = router.GetFunctionRoutes().ToEnumerable();

            // assert
            Assert.Single(routes);
        }
    }
}
