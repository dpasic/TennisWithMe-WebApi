using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;

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
    }
}