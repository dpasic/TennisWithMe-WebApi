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
    public class PlayersServiceDb : IPlayersService
    {
        [LoggerAspect]
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