﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TennisWithMe_WebApi.Models;
using Microsoft.AspNet.Identity;
using TennisWithMe_WebApi.ViewModels;
using TennisWithMe_WebApi.Services;
using AutoMapper;
using TennisWithMe_WebApi.Aspects;
using Metrics;
using TennisWithMe_WebApi.Services.Interfaces;

namespace TennisWithMe_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Players")]
    public class PlayersController : ApiController
    {
        private IPlayersService _playersService;
        private IMapper _mapperToPlayerModel;
        private IMapper _mapperToFriendship;
        private readonly Timer _timer;

        public PlayersController()
        {
            _playersService = new PlayersServiceImpl();
            _mapperToPlayerModel = new MapperConfiguration(cfg => cfg.CreateMap<Player, PlayerViewModel>()).CreateMapper();
            _mapperToFriendship = new MapperConfiguration(cfg => cfg.CreateMap<PlayersFriendshipViewModel, PlayersFriendship>()).CreateMapper();
            _timer = Metric.Timer("PlayersController.GetPlayersByQueries", Unit.Requests);
        }

        public PlayersController(IPlayersService playersService)
        {
            _playersService = playersService;
            _mapperToPlayerModel = new MapperConfiguration(cfg => cfg.CreateMap<Player, PlayerViewModel>()).CreateMapper();
            _mapperToFriendship = new MapperConfiguration(cfg => cfg.CreateMap<PlayersFriendshipViewModel, PlayersFriendship>()).CreateMapper();
            _timer = Metric.Timer("PlayersController.GetPlayersByQueries", Unit.Requests);
        }

        [HttpGet]
        [Route("")]
        [TimerAspect]
        public async Task<IHttpActionResult> GetPlayersByQueries(string city, string gender, string skill, string userID = null)
        {
            using (var context = _timer.NewContext())
            {
                string appUserID = (userID == null) ? User.Identity.GetUserId() : userID;

                city = string.IsNullOrWhiteSpace(city) ? string.Empty : city.ToLower();
                gender = string.IsNullOrWhiteSpace(gender) ? string.Empty : gender.ToLower();
                skill = string.IsNullOrWhiteSpace(skill) ? string.Empty : skill.ToLower();

                try
                {
                    var activeFriends = await _playersService.GetPlayersByQueries(appUserID, city, gender, skill);
                    var playerModels = _mapperToPlayerModel.Map<List<PlayerViewModel>>(activeFriends);

                    return Ok<List<PlayerViewModel>>(playerModels);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
