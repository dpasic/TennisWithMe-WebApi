using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Aspects;
using TennisWithMe_WebApi.Models;

namespace TennisWithMe_WebApi.Services
{
    public class PlayersService
    {
        private static PlayersService _playersService;

        public static PlayersService Instance
        {
            get
            {
                if (_playersService == null)
                {
                    _playersService = new PlayersService();
                }
                return _playersService;
            }
        }

        [TimerAspect]
        public async Task<List<Player>> GetPlayersByQueries(string appUserID, string city, string gender, string skill)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var players = db.Users.Where(x => x.Id != appUserID && x.City.ToLower().Contains(city) 
                                    && x.Gender.ToLower().Contains(gender) && x.Skill.ToLower().Contains(skill)).ToList();
                    return players;
                });
            }
        }
    }
}