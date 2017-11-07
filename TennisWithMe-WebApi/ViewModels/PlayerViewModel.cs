using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TennisWithMe_WebApi.Models;

namespace TennisWithMe_WebApi.ViewModels
{
    public class PlayerViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public int? Age { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string Club { get; set; }
        public string Summary { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }

        public Skill? SkillEnum { get; set; }
        // use EnumHelper from Booking project
        public String SkillEnumDescription
        {
            get
            {
                switch (SkillEnum)
                {
                    case Skill.Rookie:
                        return "Rookie";
                    case Skill.Amateur:
                        return "Amateur";
                    case Skill.FormerPlayer:
                        return "Former Player";
                    case Skill.Professional:
                        return "Professional";
                    default:
                        return null;
                }
            }
        }

        public double? OverallRating { get; set; } // From 1 to 5

        public int PlayedGames { get; set; }
        public int WonGames { get; set; }
        public int LostGames
        {
            get
            {
                return PlayedGames - WonGames;
            }
        }

        public int Points { get; set; }

        public bool IsFavoritePlayer
        {
            get
            {
                return (OverallRating.HasValue && OverallRating >= 4);
            }
        }

        // Points Badges
        public bool HasBronzeBadge
        {
            get
            {
                return (Points >= 200);
            }
        }

        public bool HasSilverBadge
        {
            get
            {
                return (Points >= 1000);
            }
        }

        public bool HasGoldBadge
        {
            get
            {
                return (Points >= 5000);
            }
        }

        public bool HasPlatinumBadge
        {
            get
            {
                return (Points >= 20000);
            }
        }

        // Winner Badges
        public bool HasWinnerRookieBadge
        {
            get
            {
                return (WonGames >= 5);
            }
        }

        public bool HasWinnerChallengerBadge
        {
            get
            {
                return (WonGames >= 20);
            }
        }

        public bool HasWinnerMasterBadge
        {
            get
            {
                return (WonGames >= 100);
            }
        }


        public bool IsFriendshipReceived { get; set; }
    }
}