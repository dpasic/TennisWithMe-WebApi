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
    public class MatchesServiceImpl : IMatchesService
    {
        private IEnumerable<Match> _matches;

        public MatchesServiceImpl()
        {
            _matches = null;
        }
        public MatchesServiceImpl(IEnumerable<Match> matches)
        {
            _matches = matches;
        }

        private IEnumerable<Match> GetMatches(ApplicationDbContext db)
        {
            if (_matches == null)
            {
                return db.Matches;
            }
            return _matches;
        }

        [LoggerAspect]
        public async Task<List<Match>> GetActiveMatchesForId(string appUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var matches = GetMatches(db)
                        .Where(x => (x.ChallengerId == appUserId || x.OpponentId == appUserId) && x.IsConfirmed)
                        .OrderByDescending(x => x.TimestampPlayed).ToList();
                    return matches;
                });
            }
        }

        [LoggerAspect]
        public async Task<List<Match>> GetActiveMatchesForIdAndOpponentId(string appUserId, string opponentId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var matches = GetMatches(db)
                        .Where(x => (x.ChallengerId == appUserId && x.OpponentId == opponentId || x.OpponentId == appUserId && x.ChallengerId == opponentId) && x.IsConfirmed)
                        .OrderByDescending(x => x.TimestampPlayed).ToList();
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
                    var matches = GetMatches(db)
                        .Where(x => (x.ChallengerId == appUserId || x.OpponentId == appUserId) && !x.IsConfirmed)
                        .OrderByDescending(x => x.TimestampPlayed).ToList();
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
                    var targetMatch = GetMatches(db).SingleOrDefault(x => x.Id == match.Id);
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
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    await Task.Run(() =>
                    {
                        // Update match
                        var targetMatch = GetMatches(db).SingleOrDefault(x => x.Id == model.Id);
                        var oldWinnerId = targetMatch.WinnerId;
                        var newWinnerId = model.WinnerId;

                        targetMatch.CityPlayed = model.CityPlayed;
                        targetMatch.Comment = model.CityPlayed;
                        targetMatch.Rating = model.Rating;
                        targetMatch.Result = model.Result;
                        targetMatch.WinnerId = model.WinnerId;
                        targetMatch.TimestampPlayed = model.TimestampPlayed;

                        db.SaveChanges();

                        // Update players results
                        var challenger = db.Users.SingleOrDefault(x => x.Id == model.ChallengerId);
                        var opponent = db.Users.SingleOrDefault(x => x.Id == model.OpponentId);

                        UpdateResultsForPlayer(challenger, opponent, oldWinnerId, newWinnerId);
                        UpdateResultsForPlayer(opponent, challenger, oldWinnerId, newWinnerId);

                        db.SaveChanges();

                        transaction.Commit();
                    });
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void UpdateResultsForPlayer(Player player, Player otherPlayer, string oldWinnerId, string newWinnerId)
        {
            // Prepare match results
            MatchResult oldResult, newResult;
            int oldPoints = 0, newPoints = 0;

            if (oldWinnerId == null)
            {
                oldResult = MatchResult.NotPlayed;
            }
            else
            {
                oldResult = (oldWinnerId == player.Id) ? MatchResult.Won : MatchResult.Lost;
            }

            if (newWinnerId == null)
            {
                newResult = MatchResult.NotPlayed;
            }
            else
            {
                newResult = (newWinnerId == player.Id) ? MatchResult.Won : MatchResult.Lost;
            }

            // Update results if necessary
            if (oldResult == newResult)
            {
                return;
            }
            else if (oldResult == MatchResult.NotPlayed)
            {
                player.PlayedGames += (newResult != MatchResult.NotPlayed) ? 1 : 0;
                player.WonGames += (newResult == MatchResult.Won) ? 1 : 0;

                newPoints = GetPoints(otherPlayer, newResult);
                player.Points += newPoints;
            }
            else
            {
                player.PlayedGames -= (newResult == MatchResult.NotPlayed) ? 1 : 0;

                if (oldResult == MatchResult.Won)
                {
                    player.WonGames--;
                }
                else if (oldResult == MatchResult.Lost)
                {
                    player.WonGames += (newResult == MatchResult.Won) ? 1 : 0;
                }

                oldPoints = GetPoints(otherPlayer, oldResult);
                newPoints = GetPoints(otherPlayer, newResult);
                var pointsDiff = newPoints - oldPoints;
                player.Points += pointsDiff;
            }
        }

        private static int GetPoints(Player otherPlayer, MatchResult result)
        {
            if (result == MatchResult.NotPlayed)
            {
                return 0;
            }

            var winMultiplication = (result == MatchResult.Won) ? 3 : 1;
            var points = 0;

            switch (otherPlayer.Skill)
            {
                case Skill.Rookie:
                    points = (10 * winMultiplication);
                    break;
                case Skill.Amateur:
                    points = (30 * winMultiplication);
                    break;
                case Skill.FormerPlayer:
                    points = (60 * winMultiplication);
                    break;
                case Skill.Professional:
                    points = (100 * winMultiplication);
                    break;
                default:
                    break;
            }

            return points;
        }
    }

    enum MatchResult
    {
        Won,
        Lost,
        NotPlayed
    }
}