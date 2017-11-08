using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using TennisWithMe_WebApi.Helpers;
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
        public string MobileNumber { get; set; }
        public string Club { get; set; }
        public string Summary { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }

        public Skill? Skill { get; set; }
        [NotMapped]
        public String SkillDescription
        {
            get
            {
                return EnumHelper<Skill>.GetDescriptionFromOptionalEnum(Skill);
            }
            set
            {
                Skill = EnumHelper<Skill>.GetEnumFromDescription(value);
            }
        }

        public Gender? Gender { get; set; }
        [NotMapped]
        public String GenderDescription
        {
            get
            {
                return EnumHelper<Gender>.GetDescriptionFromOptionalEnum(Gender);
            }
            set
            {
                Gender = EnumHelper<Gender>.GetEnumFromDescription(value);
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