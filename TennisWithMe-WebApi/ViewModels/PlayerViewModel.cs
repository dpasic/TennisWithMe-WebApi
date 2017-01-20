using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TennisWithMe_WebApi.Models;

namespace TennisWithMe_WebApi.ViewModels
{
    public class PlayerViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string Club { get; set; }
        public string Summary { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }

        public string Skill { get; set; }
        public double? Rating { get; set; }
    }
}