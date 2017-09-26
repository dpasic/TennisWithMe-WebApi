using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.ViewModels
{
    public class MatchViewModel
    {
        public int Id { get; set; }

        public string ChallengerId { get; set; }
        public string ChallengerName { get; set; }

        public string OpponentId { get; set; }
        public string OpponentName { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsPlayed { get; set; }

        public string Comment { get; set; }
        public string Result { get; set; }
        public string Rating { get; set; }

        public string WinnerId { get; set; }

        public string ChallengerComment { get; set; }
        public string OpponentComment { get; set; }

        public long TimestampPlayed { get; set; }
        public string CityPlayed { get; set; }

        //Used in client
        public bool IsMatchReceived { get; set; }
    }
}