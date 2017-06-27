using TennisWithMe_WebApi.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;
using TennisWithMe_WebApi.Services.Interfaces;

namespace TennisWithMe_WebApi.Services
{
    public class PlayerFriendshipsServiceImpl : IPlayerFriendshipsService
    {
        private IEnumerable<Player> _players;
        private IEnumerable<PlayersFriendship> _playersFriendships;

        public PlayerFriendshipsServiceImpl()
        {
            _players = null;
            _playersFriendships = null;
        }
        public PlayerFriendshipsServiceImpl(IEnumerable<Player> players, IEnumerable<PlayersFriendship> playersFriendships)
        {
            _players = players;
            _playersFriendships = playersFriendships;
        }

        private IEnumerable<Player> GetPlayers(ApplicationDbContext db)
        {
            if (_players == null)
            {
                return db.Users;
            }
            return _players;
        }

        private IEnumerable<PlayersFriendship> GetPlayersFriendships(ApplicationDbContext db)
        {
            if (_playersFriendships == null)
            {
                return db.PlayersFriendships;
            }
            return _playersFriendships;
        }

        [LoggerAspect]
        public async Task<List<Player>> GetActiveFriendsForId(string appUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var friendsCollection = GetPlayersFriendships(db).Where(x => (x.PlayerOneId == appUserId || x.PlayerTwoId == appUserId) && x.IsActive);
                    var friendsPart1 = friendsCollection.Where(x => x.PlayerOneId == appUserId).Select(x => x.PlayerTwo).ToList();
                    var friendsPart2 = friendsCollection.Where(x => x.PlayerTwoId == appUserId).Select(x => x.PlayerOne).ToList();

                    friendsPart1.AddRange(friendsPart2);
                    return friendsPart1;
                });
            }
        }

        [LoggerAspect]
        public async Task<List<Player>[]> GetRequestedFriendsForId(string appUserId)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var friendsCollection = GetPlayersFriendships(db).Where(x => (x.PlayerOneId == appUserId || x.PlayerTwoId == appUserId) && x.IsConfirmed == false);
                    var friendsPartRequested = friendsCollection.Where(x => x.PlayerOneId == appUserId).Select(x => x.PlayerTwo).ToList();
                    var friendsPartReceived = friendsCollection.Where(x => x.PlayerTwoId == appUserId).Select(x => x.PlayerOne).ToList();

                    return new List<Player>[] { friendsPartRequested, friendsPartReceived };
                });
            }
        }

        [LoggerAspect]
        public async Task<List<Player>> GetStrangersForQuery(string appUserID, string query)
        {
            using (var db = new ApplicationDbContext())
            {
                return await Task.Run(() =>
                {
                    var users = GetPlayers(db).Where(x => x.UserName.ToLower().Contains(query.ToLower()) && x.Id != appUserID).ToList();
                    var friendships = GetPlayersFriendships(db).Where(x => x.PlayerOneId == appUserID || x.PlayerTwoId == appUserID).ToList();

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

        [LoggerAspect]
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

        [LoggerAspect]
        public async Task ConfirmPlayersFriendship(PlayersFriendship friendship)
        {
            using (var db = new ApplicationDbContext())
            {
                await Task.Run(() =>
                {
                    var targetFriendship = GetPlayersFriendships(db).SingleOrDefault(x => (x.PlayerOneId == friendship.PlayerOneId && x.PlayerTwoId == friendship.PlayerTwoId)
                        || (x.PlayerOneId == friendship.PlayerTwoId && x.PlayerTwoId == friendship.PlayerOneId));
                    targetFriendship.IsConfirmed = true;
                    targetFriendship.IsActive = true;

                    db.SaveChanges();
                });
            }
        }
    }
}