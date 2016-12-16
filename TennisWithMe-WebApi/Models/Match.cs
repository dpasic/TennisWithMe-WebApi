﻿using System;
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

        public virtual Player PlayerOne { get; set; }
        public string PlayerOneId { get; set; }

        public virtual Player PlayerTwo { get; set; }
        public string PlayerTwoId { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsPlayed { get; set; }

        public string PlayerOneComment { get; set; }
        public string PlayerTwoComment { get; set; }

        public DateTime DatePlayed { get; set; }
        public string ClubPlayed { get; set; }
    }
}