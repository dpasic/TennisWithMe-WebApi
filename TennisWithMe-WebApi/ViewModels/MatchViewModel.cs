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
        public string PlayerOneName { get; set; }

        public string PlayerTwoId { get; set; }
        public string PlayerTwoName { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsPlayed { get; set; }

        public string Comment { get; set; }
        public string Result { get; set; }
        public string Rating { get; set; }

        public string PlayerOneComment { get; set; }
        public string PlayerTwoComment { get; set; }

        public long TimestampPlayed { get; set; }
        public string CityPlayed { get; set; }

        //Used in client
        public bool IsMatchReceived { get; set; }
    }
}