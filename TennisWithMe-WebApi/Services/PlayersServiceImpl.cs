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
    public class PlayersServiceImpl : IPlayersService
    {
        private IEnumerable<Player> _players;

        public PlayersServiceImpl()
        {
            _players = null;
        }
        public PlayersServiceImpl(IEnumerable<Player> players)
        {
            _players = players;
        }

        private IEnumerable<Player> GetPlayers(ApplicationDbContext db)
        {
            if (_players == null)
            {
                return db.Users;
            }
            return _players;
        }

        [LoggerAspect]
        public async Task<List<Player>> GetPlayersByQueries(string appUserID, string city, Gender? gender, Skill? skill)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var players = GetPlayers(db).Where(x => x.Id != appUserID
                                    && (city == null || (x.City != null && x.City.ToLower().Contains(city)))
                                    && (gender == null || x.Gender == gender)
                                    && (skill == null || x.Skill == skill)).ToList();
                    return players;
                });
            }
        }
    }
}