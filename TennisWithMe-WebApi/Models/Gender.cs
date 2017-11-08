using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.Models
{
    public enum Gender
    {
        [Description("Male")]
        Male,
        [Description("Female")]
        Female
    }
}