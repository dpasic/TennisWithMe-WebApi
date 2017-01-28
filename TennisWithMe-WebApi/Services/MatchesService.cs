using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.ViewModels;

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

        public async Task<List<Match>> GetActiveMatchesForId(string appUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var matches = db.Matches.Where(x => (x.PlayerOneId == appUserId || x.PlayerTwoId == appUserId) && x.IsConfirmed == true).ToList();
                    return matches;
                });
            }
        }

        public async Task<List<Match>> GetRequestedMatchesForId(string appUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var matches = db.Matches.Where(x => (x.PlayerOneId == appUserId || x.PlayerTwoId == appUserId) && x.IsConfirmed == false).ToList();
                    return matches;
                });
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

        public async Task ConfirmMatch(Match match)
        {
            using (var db = new ApplicationDbContext())
            {
                await Task.Run(() =>
                {
                    var targetMatch = db.Matches.Find(match.Id);
                    targetMatch.IsConfirmed = true;
                    targetMatch.IsPlayed = true;

                    db.SaveChanges();
                });
            }
        }

        public async Task UpdateMatch(MatchViewModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                await Task.Run(() =>
                {
                    var targetMatch = db.Matches.Find(model.Id);

                    targetMatch.CityPlayed = model.CityPlayed;
                    targetMatch.Comment = model.CityPlayed;
                    targetMatch.Rating = model.Rating;
                    targetMatch.Result = model.Result;
                    targetMatch.TimestampPlayed = model.TimestampPlayed;

                    db.SaveChanges();
                });
            }
        }
    }
}