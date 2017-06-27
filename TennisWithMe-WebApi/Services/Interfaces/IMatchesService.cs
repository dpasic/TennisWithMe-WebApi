using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.ViewModels;

namespace TennisWithMe_WebApi.Services.Interfaces
{
    public interface IMatchesService
    {
        Task<List<Match>> GetActiveMatchesForId(string appUserId);
        Task<List<Match>> GetRequestedMatchesForId(string appUserId);
        Task RequestMatch(Match match);
        Task ConfirmMatch(Match match);
        Task UpdateMatch(MatchViewModel model);
    }
}