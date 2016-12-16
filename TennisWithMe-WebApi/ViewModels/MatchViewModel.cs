using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.ViewModels
{
    public class MatchViewModel
    {
        public int Id { get; set; }

        public string PlayerOneId { get; set; }
        public string PlayerTwoId { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsPlayed { get; set; }

        public string PlayerOneComment { get; set; }
        public string PlayerTwoComment { get; set; }

        public DateTime DatePlayed { get; set; }
        public string ClubPlayed { get; set; }
    }
}