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
                    var friendsCollection = GetPlayersFriendships(db).Where(x => (x.RequestSenderId == appUserId || x.RequestReceiverId == appUserId) && x.IsActive);
                    var friendsPart1 = friendsCollection.Where(x => x.RequestSenderId == appUserId).ToList().Select(x => x.RequestReceiver).ToList();
                    var friendsPart2 = friendsCollection.Where(x => x.RequestReceiverId == appUserId).ToList().Select(x => x.RequestSender).ToList();

                    friendsPart1.AddRange(friendsPart2);
                    return friendsPart1.OrderBy(x => x.FirstName).ToList();
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
                    var friendsCollection = GetPlayersFriendships(db).Where(x => (x.RequestSenderId == appUserId || x.RequestReceiverId == appUserId) && !x.IsConfirmed);
                    var friendsPartRequested = friendsCollection.Where(x => x.RequestSenderId == appUserId).ToList().Select(x => x.RequestReceiver).ToList();
                    var friendsPartReceived = friendsCollection.Where(x => x.RequestReceiverId == appUserId).ToList().Select(x => x.RequestSender).ToList();

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
                    var friendships = GetPlayersFriendships(db).Where(x => x.RequestSenderId == appUserID || x.RequestReceiverId == appUserID).ToList();

                    var friendshipsIds = new HashSet<string>();
                    foreach (var item in friendships)
                    {
                        friendshipsIds.Add(item.RequestSenderId);
                        friendshipsIds.Add(item.RequestReceiverId);
                    }

                    var strangers = users.Where(x => !friendshipsIds.Contains(x.Id)).ToList();
                    return strangers.OrderBy(x => x.FirstName).ToList();
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
                    var targetFriendship = GetPlayersFriendships(db).SingleOrDefault(x => (x.RequestSenderId == friendship.RequestSenderId && x.RequestReceiverId == friendship.RequestReceiverId)
                        || (x.RequestSenderId == friendship.RequestReceiverId && x.RequestReceiverId == friendship.RequestSenderId));
                    targetFriendship.IsConfirmed = true;
                    targetFriendship.IsActive = true;

                    db.SaveChanges();
                });
            }
        }
    }
}