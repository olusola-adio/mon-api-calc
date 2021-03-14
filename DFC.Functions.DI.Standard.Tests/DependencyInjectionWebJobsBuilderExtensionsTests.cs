using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace DFC.Functions.DI.Standard.Tests
{
    public class DependencyInjectionWebJobsBuilderExtensionsTests
    {
        [Fact]
        public void AddDependencyInjection_WithWebJobsBuilderAsNull_ThrowsArgumentNullException()
        {
            // arrange
            IWebJobsBuilder builder = null;

            // act & assert
            Assert.Throws<ArgumentNullException>(() => builder.AddDependencyInjection());
        }

        [Fact]
        public void AddDependencyInjection_WithWebJobsBuilderAsNotNull_ThrowsArgumentNullException()
        {
            // arrange
            var services = new ServiceCollection();

            var mockBuilder = new Mock<IWebJobsBuilder>();
            mockBuilder.SetupGet(x => x.Services).Returns(services).Verifiable();

            // act
            var actual = mockBuilder.Object.AddDependencyInjection();

            // assert
            Assert.NotNull(actual.Services.BuildServiceProvider().GetService<IInjectBindingProvider>());
            Assert.NotNull(actual.Services.BuildServiceProvider().GetService<IFunctionFilter>());
            Assert.NotNull(actual.Services.BuildServiceProvider().GetService<IExtensionConfigProvider>());
            mockBuilder.VerifyAll();
        }
    }
}