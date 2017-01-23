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

namespace TennisWithMe_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Players")]
    public class PlayersController : ApiController
    {
        private PlayersService _playersService;
        private IMapper _mapperToPlayerModel;
        private IMapper _mapperToFriendship;

        public PlayersController()
        {
            _playersService = PlayersService.Instance;
            _mapperToPlayerModel = new MapperConfiguration(cfg => cfg.CreateMap<Player, PlayerViewModel>()).CreateMapper();
            _mapperToFriendship = new MapperConfiguration(cfg => cfg.CreateMap<PlayersFriendshipViewModel, PlayersFriendship>()).CreateMapper();
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetPlayersByQueries(string city, string gender, string skill)
        {
            string appUserID = User.Identity.GetUserId();

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