using TennisWithMe_WebApi.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.ViewModels;
using TennisWithMe_WebApi.Services.Interfaces;
using TennisWithMe_WebApi.Helpers;

namespace TennisWithMe_WebApi.Services
{
    public class IdentityPlayerServiceImpl : IIdentityPlayerService
    {
        private IEnumerable<Player> _players;

        public IdentityPlayerServiceImpl()
        {
            _players = null;
        }
        public IdentityPlayerServiceImpl(IEnumerable<Player> players)
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
        public async Task<Player> GetIdentityPlayerForId(string appUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() => GetPlayers(db).SingleOrDefault(x => x.Id == appUserId));
            }
        }

        [LoggerAspect]
        public async Task UpdateIdentityPlayerForId(string appUserId, PlayerViewModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                await Task.Run(() =>
                {
                    var targetPlayer = GetPlayers(db).SingleOrDefault(x => x.Id == appUserId);

                    targetPlayer.FirstName = model.FirstName;
                    targetPlayer.LastName = model.LastName;
                    targetPlayer.City = model.City;
                    targetPlayer.Gender = EnumHelper<Gender>.GetEnumFromDescription(model.GenderDescription);
                    targetPlayer.Skill = EnumHelper<Skill>.GetEnumFromDescription(model.SkillDescription);
                    targetPlayer.Summary = model.Summary;
                    targetPlayer.Age = model.Age;

                    db.SaveChanges();
                });
            }
        }
    }
}