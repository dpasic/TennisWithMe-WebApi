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

        public virtual Player PlayerOne { get; set; }
        public string PlayerOneId { get; set; }

        public virtual Player PlayerTwo { get; set; }
        public string PlayerTwoId { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsActive { get; set; }
    }
}