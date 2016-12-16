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
        public string Sex { get; set; }
        public string MobileNumber { get; set; }
        public string Club { get; set; }
        public string Summary { get; set; }
        public byte[] Photo { get; set; }

        public Skill Skill { get; set; }
        public double? Rating { get; set; } //From 1 to 5

        public virtual ICollection<Match> MatchesOne { get; set; }
        public virtual ICollection<Match> MatchesTwo { get; set; }

        public virtual ICollection<PlayersFriendship> PlayersOne { get; set; }
        public virtual ICollection<PlayersFriendship> PlayersTwo { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Player> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}