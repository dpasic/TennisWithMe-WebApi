using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;

namespace TennisWithMe_WebApi.Services
{
    public class MatchesService
    {
        private static MatchesService _matchesService;

        public static MatchesService Instance
        {
            get
            {
                if (_matchesService == null)
                {
                    _matchesService = new MatchesService();
                }
                return _matchesService;
            }
        }

        public async Task RequestMatch(Match match)
        {
            using (var db = new ApplicationDbContext())
            {
                await Task.Run(() =>
                {
                    db.Matches.Add(match);
                    db.SaveChanges();
                });
            }
        }
    }
}