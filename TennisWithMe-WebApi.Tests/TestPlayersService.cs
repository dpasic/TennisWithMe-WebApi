using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.Services;

namespace TennisWithMe_WebApi.Tests
{
    [TestClass]
    public class TestPlayersService
    {
        [TestMethod]
        public async Task GetAllPlayers_ShouldReturnAllPlayers()
        {
            var testPlayers = GetTestPlayers();
            var service = new PlayersServiceLocal(testPlayers);

            var result = await service.GetPlayersByQueries(string.Empty, string.Empty, string.Empty, string.Empty);
            Assert.AreEqual(testPlayers.Count(), result.Count);
        }

        [TestMethod]
        public async Task GetPlayersFromZagreb_ShouldReturnPlayersFromZagreb()
        {
            var testPlayers = GetTestPlayers();
            var service = new PlayersServiceLocal(testPlayers);

            var result = await service.GetPlayersByQueries(string.Empty, "zagreb", string.Empty, string.Empty);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetPlayersByQueries_ShouldReturnPlayersByQueries()
        {
            var testPlayers = GetTestPlayers();
            var service = new PlayersServiceLocal(testPlayers);

            var result = await service.GetPlayersByQueries(string.Empty, "rijeka", "male", "former player");
            Assert.AreEqual(1, result.Count);
        }

        private IEnumerable<Player> GetTestPlayers()
        {
            var players = new List<Player>();
            players.Add(new Player { City = "Pula", Gender = "Male", Skill = "Rookie" });
            players.Add(new Player { City = "Zagreb", Gender = "Male", Skill = "Former Player" });
            players.Add(new Player { City = "Zagreb", Gender = "Female", Skill = "Rookie" });
            players.Add(new Player { City = "Rijeka", Gender = "Male", Skill = "Former Player" });

            return players;
        }
    }
}
