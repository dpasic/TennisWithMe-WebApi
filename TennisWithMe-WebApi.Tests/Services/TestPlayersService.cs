using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.Services;

namespace TennisWithMe_WebApi.Tests.Services
{
    [TestClass]
    public class TestPlayersService
    {
        [TestMethod]
        public async Task GetAllPlayers_ShouldReturnAllPlayers()
        {
            var testPlayers = GetTestPlayers();
            var service = new PlayersServiceImpl(testPlayers);

            var result = await service.GetPlayersByQueries(string.Empty, string.Empty, null, null);
            Assert.AreEqual(testPlayers.Count(), result.Count);
        }

        [TestMethod]
        public async Task GetPlayersFromZagreb_ShouldReturnPlayersFromZagreb()
        {
            var testPlayers = GetTestPlayers();
            var service = new PlayersServiceImpl(testPlayers);

            var result = await service.GetPlayersByQueries(string.Empty, "zagreb", null, null);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetPlayersByQueries_ShouldReturnPlayersByQueries()
        {
            var testPlayers = GetTestPlayers();
            var service = new PlayersServiceImpl(testPlayers);

            var result = await service.GetPlayersByQueries(string.Empty, "rijeka", Gender.Male, Skill.FormerPlayer);
            Assert.AreEqual(1, result.Count);
        }

        private IEnumerable<Player> GetTestPlayers()
        {
            var players = new List<Player>();
            players.Add(new Player { City = "Pula", Gender = Gender.Male, Skill = Skill.Rookie });
            players.Add(new Player { City = "Zagreb", Gender = Gender.Male, Skill = Skill.FormerPlayer });
            players.Add(new Player { City = "Zagreb", Gender = Gender.Female, Skill = Skill.Rookie });
            players.Add(new Player { City = "Rijeka", Gender = Gender.Male, Skill = Skill.FormerPlayer });

            return players;
        }
    }
}
