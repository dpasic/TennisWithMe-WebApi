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
                    var matches = GetMatches(db).Where(x => (x.ChallengerId == appUserId || x.OpponentId == appUserId) && x.IsConfirmed).ToList();
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
                    var matches = GetMatches(db).Where(x => (x.ChallengerId == appUserId && x.OpponentId == opponentId || x.OpponentId == appUserId && x.ChallengerId == opponentId) && x.IsConfirmed).ToList();
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
                    var matches = GetMatches(db).Where(x => (x.ChallengerId == appUserId || x.OpponentId == appUserId) && !x.IsConfirmed).ToList();
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

                        targetMatch.CityPlayed = model.CityPlayed;
                        targetMatch.Comment = model.CityPlayed;
                        targetMatch.Rating = model.Rating;
                        targetMatch.Result = model.Result;
                        targetMatch.WinnerId = model.WinnerId;
                        targetMatch.TimestampPlayed = model.TimestampPlayed;

                        db.SaveChanges();

                        // Update players results
                        var confirmedMatchesChallenger = GetMatches(db).Where(x => (x.ChallengerId == model.ChallengerId || x.OpponentId == model.ChallengerId) && x.IsConfirmed).ToList();
                        var confirmedMatchesOpponent = GetMatches(db).Where(x => (x.ChallengerId == model.OpponentId || x.OpponentId == model.OpponentId) && x.IsConfirmed).ToList();

                        var challenger = db.Users.SingleOrDefault(x => x.Id == model.ChallengerId);
                        var opponent = db.Users.SingleOrDefault(x => x.Id == model.OpponentId);

                        UpdateResultsForPlayer(challenger, opponent, confirmedMatchesChallenger);
                        UpdateResultsForPlayer(opponent, challenger, confirmedMatchesOpponent);

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

        // TODO: ispravi ovo da dohvati sve igrace s kojim je ovaj ikad igrao pa se otherPlayer prilagodjava tome
        private void UpdateResultsForPlayer(Player player, Player otherPlayer, List<Match> confirmedMatches)
        {
            int wons = 0, losses = 0, points = 0;
            foreach (var match in confirmedMatches)
            {
                var isWinner = false;

                if (match.WinnerId == player.Id)
                {
                    wons++;
                    isWinner = true;
                }
                else if (match.WinnerId != null)
                {
                    losses++;
                }

                var winMultiplication = isWinner ? 3 : 1;
                switch (otherPlayer.Skill)
                {
                    case Skill.Rookie:
                        points += (10 * winMultiplication);
                        break;
                    case Skill.Amateur:
                        points += (30 * winMultiplication);
                        break;
                    case Skill.FormerPlayer:
                        points += (60 * winMultiplication);
                        break;
                    case Skill.Professional:
                        points += (100 * winMultiplication);
                        break;
                    default:
                        break;
                }
            }

            player.PlayedGames = wons + losses;
            player.WonGames = wons;
            player.Points = points;
        }
    }
}