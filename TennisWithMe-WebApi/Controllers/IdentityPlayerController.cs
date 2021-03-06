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
using TennisWithMe_WebApi.Services.Interfaces;
using Metrics;

namespace TennisWithMe_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/IdentityPlayer")]
    public class IdentityPlayerController : ApiController
    {
        private IIdentityPlayerService _identityPlayerService;
        private IMapper _mapper;
        private readonly Counter _counter;

        public IdentityPlayerController()
        {
            _identityPlayerService = new IdentityPlayerServiceImpl();
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Player, PlayerViewModel>()).CreateMapper();
            _counter = Metric.Counter("IdentityPlayerController.GetIdentityPlayer", Unit.Calls);
        }

        public IdentityPlayerController(IIdentityPlayerService identityPlayerService)
        {
            _identityPlayerService = identityPlayerService;
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Player, PlayerViewModel>()).CreateMapper();
            _counter = Metric.Counter("IdentityPlayerController.GetIdentityPlayer", Unit.Calls);
        }

        [HttpGet]
        [Route("")]
        [TimerAspect]
        public async Task<IHttpActionResult> GetIdentityPlayer(string userID = null)
        {
            _counter.Increment();

            string appUserID = (userID == null) ? User.Identity.GetUserId() : userID;

            try
            {
                var identityPlayer = await _identityPlayerService.GetIdentityPlayerForId(appUserID);
                var identityPlayerModel = _mapper.Map<PlayerViewModel>(identityPlayer);

                return Ok<PlayerViewModel>(identityPlayerModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("")]
        [TimerAspect]
        public async Task<IHttpActionResult> UpdateIdentityPlayer(PlayerViewModel model, string userID = null)
        {
            string appUserID = (userID == null) ? User.Identity.GetUserId() : userID;

            try
            {
                await _identityPlayerService.UpdateIdentityPlayerForId(appUserID, model);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
