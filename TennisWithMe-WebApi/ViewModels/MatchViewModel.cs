using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TennisWithMe_WebApi.Helpers;
using TennisWithMe_WebApi.Models;

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

        public Rating? Rating { get; set; }
        public String RatingDescription
        {
            get
            {
                return EnumHelper<Rating>.GetDescriptionFromOptionalEnum(Rating);
            }
            set
            {
                Rating = EnumHelper<Rating>.GetEnumFromDescription(value);
            }
        }
        public int RatingValue
        {
            get
            {
                if (Rating == null)
                {
                    return 0;
                }
                return (int)Rating;
            }
        }

        public string WinnerId { get; set; }
        public string WinnerName
        {
            get
            {
                if (WinnerId == ChallengerId)
                {
                    return ChallengerName;
                }
                else if (WinnerId == OpponentId)
                {
                    return OpponentName;
                }
                else
                {
                    return null;
                }
            }
        }

        public string ChallengerComment { get; set; }
        public string OpponentComment { get; set; }

        public long TimestampPlayed { get; set; }
        public string CityPlayed { get; set; }

        //Used in client
        public bool IsMatchReceived { get; set; }
    }
}