using TennisWithMe_WebApi.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.ViewModels;
using TennisWithMe_WebApi.Services.Interfaces;

namespace TennisWithMe_WebApi.Services
{
    public class MatchesServiceDb : IMatchesService
    {
        [LoggerAspect]
        public async Task<List<Match>> GetActiveMatchesForId(string appUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var matches = db.Matches.Where(x => (x.PlayerOneId == appUserId || x.PlayerTwoId == appUserId) && x.IsConfirmed).ToList();
                    return matches;
                });
            }
        }

        [LoggerAspect]
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

        [LoggerAspect]
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

        [LoggerAspect]
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

        [LoggerAspect]
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