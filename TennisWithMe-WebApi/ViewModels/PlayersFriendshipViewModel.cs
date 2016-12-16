using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.ViewModels
{
    public class PlayersFriendshipViewModel
    {
        public int Id { get; set; }

        public string PlayerOneId { get; set; }
        public string PlayerTwoId { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsActive { get; set; }
    }
}