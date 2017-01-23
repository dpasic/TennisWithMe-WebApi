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

namespace TennisWithMe_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Matches")]
    public class MatchesController : ApiController
    {
        private MatchesService _matchesService;
        private IMapper _mapper;

        public MatchesController()
        {
            _matchesService = MatchesService.Instance;
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Match, MatchViewModel>()).CreateMapper();
        }

        //[HttpGet]
        //[Route("Active")]
        //public async Task<IHttpActionResult> GetActiveFriends()
        //{
        //    string appUserID = User.Identity.GetUserId();

        //    try
        //    {
        //        var activeFriends = await _playerFriendshipsService.GetActiveFriendsForId(appUserID);
        //        var playerModels = _mapperToPlayerModel.Map<List<PlayerViewModel>>(activeFriends);

        //        return Ok<List<PlayerViewModel>>(playerModels);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("Requested")]
        //public async Task<IHttpActionResult> GetRequestedFriends()
        //{
        //    string appUserID = User.Identity.GetUserId();

        //    try
        //    {
        //        var requestedFriends = await _playerFriendshipsService.GetRequestedFriendsForId(appUserID);
        //        var playerModels = _mapperToPlayerModel.Map<List<PlayerViewModel>>(requestedFriends);

        //        return Ok<List<PlayerViewModel>>(playerModels);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        [Route("RequestMatch")]
        public async Task<IHttpActionResult> RequestMatch(MatchViewModel model)
        {
            string appUserID = User.Identity.GetUserId();
            model.PlayerOneId = (model.PlayerOneId == null) ? appUserID : model.PlayerOneId;

            try
            {
                var match = _mapper.Map<Match>(model);
                await _matchesService.RequestMatch(match);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPut]
        //[Route("ConfirmFriendship")]
        //public async Task<IHttpActionResult> ConfirmFriendship(PlayersFriendshipViewModel model)
        //{
        //    string appUserID = User.Identity.GetUserId();
        //    model.PlayerOneId = (model.PlayerOneId == null) ? appUserID : model.PlayerOneId;

        //    try
        //    {
        //        var friendship = _mapperToFriendship.Map<PlayersFriendship>(model);
        //        await _playerFriendshipsService.ConfirmPlayersFriendship(friendship);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
