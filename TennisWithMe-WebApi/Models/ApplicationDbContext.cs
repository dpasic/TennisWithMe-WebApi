using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.Models
{
    //READ: https://www.asp.net/web-api/overview/security/individual-accounts-in-web-api
    public class ApplicationDbContext : IdentityDbContext<Player>
    {
        public DbSet<PlayersFriendship> PlayersFriendships { get; set; }
        public DbSet<Match> Matches { get; set; }

        public ApplicationDbContext()
            : base("ApplicationDbContext", throwIfV1Schema: false)
        { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Match
            modelBuilder.Entity<Match>()
                    .HasRequired(m => m.PlayerOne)
                    .WithMany(t => t.MatchesOne)
                    .HasForeignKey(m => m.PlayerOneId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Match>()
                    .HasRequired(m => m.PlayerTwo)
                    .WithMany(t => t.MatchesTwo)
                    .HasForeignKey(m => m.PlayerTwoId)
                    .WillCascadeOnDelete(false);

            // PlayersFriendship
            modelBuilder.Entity<PlayersFriendship>()
                    .HasRequired(m => m.PlayerOne)
                    .WithMany(t => t.PlayersOne)
                    .HasForeignKey(m => m.PlayerOneId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlayersFriendship>()
                    .HasRequired(m => m.PlayerTwo)
                    .WithMany(t => t.PlayersTwo)
                    .HasForeignKey(m => m.PlayerTwoId)
                    .WillCascadeOnDelete(false);

            // PlayersRating
            modelBuilder.Entity<PlayersRating>()
                    .HasRequired(m => m.Reviewer)
                    .WithMany(t => t.Reviewers)
                    .HasForeignKey(m => m.ReviewerId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlayersRating>()
                    .HasRequired(m => m.Rated)
                    .WithMany(t => t.Rated)
                    .HasForeignKey(m => m.RatedId)
                    .WillCascadeOnDelete(false);
        }
    }
}