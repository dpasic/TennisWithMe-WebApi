using System;
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

namespace TennisWithMe_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/IdentityPlayer")]
    public class IdentityPlayerController : ApiController
    {
        private IIdentityPlayerService _identityPlayerService;
        private IMapper _mapper;

        public IdentityPlayerController()
        {
            _identityPlayerService = new IdentityPlayerServiceDb();
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Player, PlayerViewModel>()).CreateMapper();
        }

        public IdentityPlayerController(IIdentityPlayerService identityPlayerService)
        {
            _identityPlayerService = identityPlayerService;
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Player, PlayerViewModel>()).CreateMapper();
        }

        [HttpGet]
        [Route("")]
        [TimerAspect]
        public async Task<IHttpActionResult> GetIdentityPlayer(string userID = null)
        {
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
