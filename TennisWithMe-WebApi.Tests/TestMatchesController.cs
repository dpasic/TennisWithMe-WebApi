using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using TennisWithMe_WebApi.Controllers;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.Services;
using TennisWithMe_WebApi.Services.Interfaces;
using TennisWithMe_WebApi.ViewModels;

namespace TennisWithMe_WebApi.Tests
{
    [TestClass]
    public class TestMatchesController
    {
        private static string MOCK_ID = "testID";

        [TestMethod]
        public async Task GetReturnsMatchesWithSameId()
        {
            // Arrange
            var mockService = new Mock<IMatchesService>();
            mockService.Setup(x => x.GetActiveMatchesForId(MOCK_ID)).ReturnsAsync(GetTestMatches());

            var controller = new MatchesController(mockService.Object);

            // Act
            var actionResult = await controller.GetActiveMatches(MOCK_ID);
            var contentResult = actionResult as OkNegotiatedContentResult<List<MatchViewModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            foreach (var item in contentResult.Content)
            {
                Assert.AreEqual(MOCK_ID, item.PlayerOneId);
            }
        }

        private List<Models.Match> GetTestMatches()
        {
            var matches = new List<Models.Match>();
            matches.Add(new Models.Match { PlayerOneId = MOCK_ID, PlayerTwoId = "secondID" });
            matches.Add(new Models.Match { PlayerOneId = MOCK_ID, PlayerTwoId = "thirdID" });

            return matches;
        }
    }
}
