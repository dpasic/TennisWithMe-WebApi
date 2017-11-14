using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.ViewModels;

namespace TennisWithMe_WebApi.Services.Interfaces
{
    public interface IPlayerRatingsService
    {
        Task<PlayersRating> GetPlayersRatingForIdAndPlayerId(string appUserID, string friendId);
        Task CreateOrUpdatePlayersRating(PlayersRatingViewModel model, PlayersRating entity);
    }
}