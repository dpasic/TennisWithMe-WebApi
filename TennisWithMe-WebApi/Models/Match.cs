using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.Models
{
    [Table("Match")]
    public class Match
    {
        public int Id { get; set; }

        public virtual Player Challenger { get; set; }
        public string ChallengerId { get; set; }
        public string ChallengerName { get; set; }

        public virtual Player Opponent { get; set; }
        public string OpponentId { get; set; }
        public string OpponentName { get; set; }

        public string WinnerId { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsPlayed { get; set; }

        public string Comment { get; set; }
        public string Result { get; set; }
        public Rating? Rating { get; set; }

        public string ChallengerComment { get; set; }
        public string OpponentComment { get; set; }

        public long TimestampPlayed { get; set; }
        public string CityPlayed { get; set; }
    }
}