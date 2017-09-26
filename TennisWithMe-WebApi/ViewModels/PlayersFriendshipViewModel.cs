using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.ViewModels
{
    public class PlayersFriendshipViewModel
    {
        public int Id { get; set; }

        public string RequestSender { get; set; }
        public string RequestReceiver { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsActive { get; set; }
    }
}