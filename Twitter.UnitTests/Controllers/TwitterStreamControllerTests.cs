using JH.Twitter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TwitterStatistics.Controllers;

namespace Twitter.UnitTests.Controllers
{
    [TestClass]
    public class TwitterStreamControllerTests
    {
        
        [TestMethod]
        public void GetStatisticsForValidOutput()
        {
            var exprectedResult = new System.Collections.Generic.Dictionary<string, string>();
            exprectedResult.Add("Sample metric", "321");
            var mockRepo = new Mock<IAnalyticsGenerator>();
            mockRepo.Setup(repo => repo.GenerateAnalytics())
                .Returns(exprectedResult);

            var controller = new TwitterStatisticsController(mockRepo.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.GetStatistics();

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void GetStatistics_Exception()
        {
            var mockRepo = new Mock<IAnalyticsGenerator>();
            mockRepo.Setup(repo => repo.GenerateAnalytics())
                .Throws(new System.Exception("Test exception"));

            var controller = new TwitterStatisticsController(mockRepo.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var result = controller.GetStatistics();

            Assert.AreEqual(result.StatusCode, HttpStatusCode.InternalServerError);
        }
    }
}
