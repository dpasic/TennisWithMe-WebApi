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

namespace TennisWithMe_WebApi.Tests.Controllers
{
    [TestClass]
    public class TestPlayersController
    {
        [TestMethod]
        public async Task GetReturnsPlayersByQuery()
        {
            // Arrange
            var controller = new PlayersController(new PlayersServiceImpl(GetTestPlayers()));

            // Act
            var actionResult = await controller.GetPlayersByQueries("Zagreb", null, "Former Player", "testID");
            var contentResult = actionResult as OkNegotiatedContentResult<List<PlayerViewModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.Count == 2);

            foreach (var item in contentResult.Content)
            {
                Assert.AreEqual("Zagreb", item.City);
                Assert.AreEqual("Former Player", item.Skill);
            }
        }

        private IEnumerable<Player> GetTestPlayers()
        {
            var players = new List<Player>();
            players.Add(new Player { City = "Pula", Gender = "Male", Skill = "Rookie" });
            players.Add(new Player { City = "Zagreb", Gender = "Male", Skill = "Former Player" });
            players.Add(new Player { City = "Zagreb", Gender = "Female", Skill = "Rookie" });
            players.Add(new Player { City = "Rijeka", Gender = "Male", Skill = "Former Player" });
            players.Add(new Player { City = "Zagreb", Gender = "Female", Skill = "Former Player" });

            return players;
        }
    }
}
