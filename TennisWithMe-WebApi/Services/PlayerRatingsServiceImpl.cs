using TennisWithMe_WebApi.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.Services.Interfaces;
using TennisWithMe_WebApi.ViewModels;

namespace TennisWithMe_WebApi.Services
{
    public class PlayerRatingsServiceImpl : IPlayerRatingsService
    {
        private IEnumerable<PlayersRating> _playersRatings;

        public PlayerRatingsServiceImpl()
        {
            _playersRatings = null;
        }
        public PlayerRatingsServiceImpl(IEnumerable<PlayersRating> playersRatings)
        {
            _playersRatings = playersRatings;
        }

        public async Task<PlayersRating> GetPlayersRatingForIdAndPlayerId(string appUserID, string friendId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var playersRating = db.PlayersRatings.SingleOrDefault(x => x.ReviewerId == appUserID && x.RatedId == friendId);
                    return playersRating;
                });
            }
        }

        public async Task CreateOrUpdatePlayersRating(PlayersRatingViewModel model, PlayersRating entity)
        {
            using (var db = new ApplicationDbContext())
            using (var transaction = db.Database.BeginTransaction())
            {
                await Task.Run(() =>
                {
                    var playersRating = db.PlayersRatings.SingleOrDefault(x => x.ReviewerId == model.ReviewerId && x.RatedId == model.RatedId);

                    // Add or update rating
                    if (playersRating == null)
                    {
                        db.PlayersRatings.Add(entity);
                    }
                    else
                    {
                        playersRating.Rating = model.Rating;
                    }

                    db.SaveChanges();

                    // Update players overall rating
                    var ratedPlayer = db.Users.SingleOrDefault(x => x.Id == model.RatedId);
                    var ratedPlayerRatings = db.PlayersRatings.Where(x => x.RatedId == model.RatedId).ToList();

                    int counter = 0, ratingSum = 0;
                    foreach (var item in ratedPlayerRatings)
                    {
                        var ratingInt = (int)item.Rating;
                        if (ratingInt == 0)
                        {
                            continue;
                        }

                        counter++;
                        ratingSum += ratingInt;
                    }

                    ratedPlayer.OverallRating = ratingSum / (double)counter;

                    db.SaveChanges();
                    transaction.Commit();
                });
            }
        }
    }
}