﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TennisWithMe_WebApi.Models;

namespace TennisWithMe_WebApi.Services.Interfaces
{
    public interface IPlayersService
    {
        Task<List<Player>> GetPlayersByQueries(string appUserID, string city, string gender, string skill);
    }
}