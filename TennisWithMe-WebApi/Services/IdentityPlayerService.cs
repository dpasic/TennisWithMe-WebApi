using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.ViewModels;

namespace TennisWithMe_WebApi.Services
{
    public class IdentityPlayerService
    {
        private static IdentityPlayerService _identityPlayerService;

        public static IdentityPlayerService Instance
        {
            get
            {
                if (_identityPlayerService == null)
                {
                    _identityPlayerService = new IdentityPlayerService();
                }
                return _identityPlayerService;
            }
        }

        public async Task<Player> GetIdentityPlayerForId(string appUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() => db.Users.Find(appUserId));
            }
        }

        public async Task UpdateIdentityPlayerForId(string appUserId, PlayerViewModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                await Task.Run(() =>
                {
                    var targetPlayer = db.Users.Find(appUserId);

                    targetPlayer.FirstName = model.FirstName;
                    targetPlayer.LastName = model.LastName;
                    targetPlayer.City = model.City;
                    targetPlayer.Gender = model.Gender;
                    targetPlayer.Skill = model.Skill;
                    targetPlayer.Summary = model.Summary;
                    targetPlayer.Age = model.Age;

                    db.SaveChanges();
                });
            }
        }
    }
}