using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.ViewModels;

namespace TennisWithMe_WebApi.Services.Interfaces
{
    public interface IIdentityPlayerService
    {
        Task<Player> GetIdentityPlayerForId(string appUserId);
        Task UpdateIdentityPlayerForId(string appUserId, PlayerViewModel model);
    }
}