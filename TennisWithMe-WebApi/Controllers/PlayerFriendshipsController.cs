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
    [RoutePrefix("api/PlayerFriendships")]
    public class PlayerFriendshipsController : ApiController
    {
        private PlayerFriendshipsService _playerFriendshipsService;
        private IMapper _mapperToPlayerModel;
        private IMapper _mapperToFriendship;

        public PlayerFriendshipsController()
        {
            _playerFriendshipsService = PlayerFriendshipsService.Instance;
            _mapperToPlayerModel = new MapperConfiguration(cfg => cfg.CreateMap<Player, PlayerViewModel>()).CreateMapper();
            _mapperToFriendship = new MapperConfiguration(cfg => cfg.CreateMap<PlayersFriendshipViewModel, PlayersFriendship>()).CreateMapper();
        }

        [HttpGet]
        [Route("Active")]
        public async Task<IHttpActionResult> GetActiveFriends()
        {
            string appUserID = User.Identity.GetUserId();

            try
            {
                var activeFriends = await _playerFriendshipsService.GetActiveFriendsForId(appUserID);
                var playerModels = _mapperToPlayerModel.Map<List<PlayerViewModel>>(activeFriends);

                return Ok<List<PlayerViewModel>>(playerModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Requested")]
        public async Task<IHttpActionResult> GetRequestedFriends()
        {
            string appUserID = User.Identity.GetUserId();

            try
            {
                var requestedFriends = await _playerFriendshipsService.GetRequestedFriendsForId(appUserID);
                var playerModels = _mapperToPlayerModel.Map<List<PlayerViewModel>>(requestedFriends);

                return Ok<List<PlayerViewModel>>(playerModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("RequestFriendship")]
        public async Task<IHttpActionResult> RequestFriendship(PlayersFriendshipViewModel model)
        {
            string appUserID = User.Identity.GetUserId();

            try
            {
                var friendship = _mapperToFriendship.Map<PlayersFriendship>(model);
                await _playerFriendshipsService.RequestPlayersFriendship(friendship);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("ConfirmFriendship")]
        public async Task<IHttpActionResult> ConfirmFriendship(PlayersFriendshipViewModel model)
        {
            string appUserID = User.Identity.GetUserId();

            try
            {
                var friendship = _mapperToFriendship.Map<PlayersFriendship>(model);
                await _playerFriendshipsService.ConfirmPlayersFriendship(friendship);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}