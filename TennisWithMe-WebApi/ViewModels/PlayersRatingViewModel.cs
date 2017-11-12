using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TennisWithMe_WebApi.Helpers;
using TennisWithMe_WebApi.Models;

namespace TennisWithMe_WebApi.ViewModels
{
    public class PlayersRatingViewModel
    {
        public int Id { get; set; }

        public string ReviewerId { get; set; }
        public string RatedId { get; set; }

        public Rating Rating { get; set; }
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
                return (int)Rating;
            }
        }
    }
}