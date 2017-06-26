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

namespace TennisWithMe_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/IdentityPlayer")]
    public class IdentityPlayerController : ApiController
    {
        private IdentityPlayerService _identityPlayerService;
        private IMapper _mapper;

        public IdentityPlayerController()
        {
            _identityPlayerService = IdentityPlayerService.Instance;
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Player, PlayerViewModel>()).CreateMapper();
        }

        [HttpGet]
        [Route("")]
        [TimerAspect]
        public async Task<IHttpActionResult> GetIdentityPlayer()
        {
            string appUserId = User.Identity.GetUserId();

            try
            {
                var identityPlayer = await _identityPlayerService.GetIdentityPlayerForId(appUserId);
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
        public async Task<IHttpActionResult> UpdateIdentityPlayer(PlayerViewModel model)
        {
            string appUserId = User.Identity.GetUserId();

            try
            {
                await _identityPlayerService.UpdateIdentityPlayerForId(appUserId, model);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
