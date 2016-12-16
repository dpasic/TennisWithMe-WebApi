using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;

namespace TennisWithMe_WebApi.Services
{
    public class PlayerFriendshipsService
    {
        private static PlayerFriendshipsService _playerFriendshipsService;

        public static PlayerFriendshipsService Instance
        {
            get
            {
                if (_playerFriendshipsService == null)
                {
                    _playerFriendshipsService = new PlayerFriendshipsService();
                }
                return _playerFriendshipsService;
            }
        }

        public async Task<List<Player>> GetActiveFriendsForId(string appUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var friends = db.PlayersFriendships.Where(x => x.PlayerOneId == appUserId && x.IsActive == true).Select(x => x.PlayerTwo).ToList();
                    return friends;
                });
            }
        }

        public async Task<List<Player>> GetRequestedFriendsForId(string appUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var friends = db.PlayersFriendships.Where(x => x.PlayerOneId == appUserId && x.IsConfirmed == false).Select(x => x.PlayerTwo).ToList();
                    return friends;
                });
            }
        }

        public async Task RequestPlayersFriendship(PlayersFriendship friendship)
        {
            using (var db = new ApplicationDbContext())
            {
                await Task.Run(() =>
                {
                    db.PlayersFriendships.Add(friendship);
                    db.SaveChanges();
                });
            }
        }

        public async Task ConfirmPlayersFriendship(PlayersFriendship friendship)
        {
            using (var db = new ApplicationDbContext())
            {
                await Task.Run(() =>
                {
                    var targetFriendship = db.PlayersFriendships.SingleOrDefault(x => x.PlayerOneId == friendship.PlayerOneId && x.PlayerTwoId == friendship.PlayerTwoId);
                    targetFriendship.IsConfirmed = true;
                    targetFriendship.IsActive = true;

                    db.SaveChanges();
                });
            }
        }
    }
}