using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.Models
{
    public enum Rating
    {
        [Description("Undefined")]
        Undefined,
        [Description("1 star")]
        OneStar,
        [Description("2 stars")]
        TwoStars,
        [Description("3 stars")]
        ThreeStars,
        [Description("4 stars")]
        FourStars,
        [Description("5 stars")]
        FiveStars
    }
}