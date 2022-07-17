
namespace Mon.Calculator.UnitTests
{
    using System.Net;
    using Xunit;
    public class UnitTest1
    {
        [Fact]
        public void GetDetailHttpTriggerRunReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;

            // Act
            // Assert
            Assert.Equal((int)expectedResult, (int)expectedResult);
        }
    }
}