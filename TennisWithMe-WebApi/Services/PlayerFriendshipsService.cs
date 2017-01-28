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
                    var friendsCollection = db.PlayersFriendships.Where(x => (x.PlayerOneId == appUserId || x.PlayerTwoId == appUserId) && x.IsActive == true);
                    var friendsPart1 = friendsCollection.Where(x => x.PlayerOneId == appUserId).Select(x => x.PlayerTwo).ToList();
                    var friendsPart2 = friendsCollection.Where(x => x.PlayerTwoId == appUserId).Select(x => x.PlayerOne).ToList();

                    friendsPart1.AddRange(friendsPart2);
                    return friendsPart1;
                });
            }
        }

        public async Task<List<Player>[]> GetRequestedFriendsForId(string appUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var friendsCollection = db.PlayersFriendships.Where(x => (x.PlayerOneId == appUserId || x.PlayerTwoId == appUserId) && x.IsConfirmed == false);
                    var friendsPartRequested = friendsCollection.Where(x => x.PlayerOneId == appUserId).Select(x => x.PlayerTwo).ToList();
                    var friendsPartReceived = friendsCollection.Where(x => x.PlayerTwoId == appUserId).Select(x => x.PlayerOne).ToList();

                    return new List<Player>[] { friendsPartRequested, friendsPartReceived };
                });
            }
        }

        public async Task<List<Player>> GetStrangersForQuery(string appUserID, string query)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var users = db.Users.Where(x => x.UserName.ToLower().Contains(query.ToLower()) && x.Id != appUserID).ToList();
                    var friendships = db.PlayersFriendships.Where(x => x.PlayerOneId == appUserID || x.PlayerTwoId == appUserID).ToList();

                    var friendshipsIds = new HashSet<string>();
                    foreach (var item in friendships)
                    {
                        friendshipsIds.Add(item.PlayerOneId);
                        friendshipsIds.Add(item.PlayerTwoId);
                    }

                    var strangers = users.Where(x => !friendshipsIds.Contains(x.Id)).ToList();
                    return strangers;
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