using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace TennisWithMe_WebApi.Models
{
    public class Player : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string City { get; set; }
        public string MobileNumber { get; set; }
        public string Club { get; set; }
        public string Summary { get; set; }
        public byte[] Photo { get; set; }

        public Gender? Gender { get; set; }
        public Skill? Skill { get; set; }

        public double? OverallRating { get; set; } // From 1 to 5

        public int PlayedGames { get; set; }
        public int WonGames { get; set; }

        public int Points { get; set; }

        public virtual ICollection<Match> ChallengersMatches { get; set; }
        public virtual ICollection<Match> OpponentsMatches { get; set; }

        public virtual ICollection<PlayersFriendship> RequestsSenders { get; set; }
        public virtual ICollection<PlayersFriendship> RequestsReceivers { get; set; }

        public virtual ICollection<PlayersRating> Reviewers { get; set; }
        public virtual ICollection<PlayersRating> Rated { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Player> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}