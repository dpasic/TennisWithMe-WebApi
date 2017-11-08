using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.Models
{
    public enum Skill
    {
        [Description("Rookie")]
        Rookie,
        [Description("Amateur")]
        Amateur,
        [Description("Professional")]
        Professional,
        [Description("Former Player")]
        FormerPlayer
    }
}