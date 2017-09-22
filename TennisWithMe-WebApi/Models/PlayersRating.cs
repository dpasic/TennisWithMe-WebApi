using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.Models
{
    [Table("PlayersRating")]
    public class PlayersRating
    {
        public int Id { get; set; }

        public virtual Player Reviewer { get; set; }
        public string ReviewerId { get; set; }

        public virtual Player Rated { get; set; }
        public string RatedId { get; set; }

        public double Rating { get; set; } // From 1 to 5
    }
}