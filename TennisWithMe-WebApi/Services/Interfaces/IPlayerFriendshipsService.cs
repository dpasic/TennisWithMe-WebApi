using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisWithMe_WebApi.Models;

namespace TennisWithMe_WebApi.Services.Interfaces
{
    public interface IPlayerFriendshipsService
    {
        Task<List<Player>> GetActiveFriendsForId(string appUserId);
        Task<List<Player>[]> GetRequestedFriendsForId(string appUserId);
        Task<List<Player>> GetStrangersForQuery(string appUserID, string query);
        Task RequestPlayersFriendship(PlayersFriendship friendship);
        Task ConfirmPlayersFriendship(PlayersFriendship friendship);
    }
}
