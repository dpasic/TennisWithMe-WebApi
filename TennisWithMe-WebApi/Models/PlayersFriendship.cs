using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.Models
{
    [Table("PlayersFriendship")]
    public class PlayersFriendship
    {
        public int Id { get; set; }

        public virtual Player RequestSender { get; set; }
        public string RequestSenderId { get; set; }

        public virtual Player RequestReceiver { get; set; }
        public string RequestReceiverId { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsActive { get; set; }
    }
}