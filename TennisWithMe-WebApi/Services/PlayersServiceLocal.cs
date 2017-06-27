using TennisWithMe_WebApi.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.Services.Interfaces;

namespace TennisWithMe_WebApi.Services
{
    public class PlayersServiceLocal : IPlayersService
    {
        private IEnumerable<Player> _players;

        public PlayersServiceLocal(IEnumerable<Player> players)
        {
            _players = players;
        }

        [LoggerAspect]
        public async Task<List<Player>> GetPlayersByQueries(string appUserID, string city, string gender, string skill)
        {
            return await Task.Run(() =>
            {
                var players = _players.Where(x => x.Id != appUserID && x.City.ToLower().Contains(city)
                                && x.Gender.ToLower().Contains(gender) && x.Skill.ToLower().Contains(skill)).ToList();
                return players;
            });
        }
    }
}