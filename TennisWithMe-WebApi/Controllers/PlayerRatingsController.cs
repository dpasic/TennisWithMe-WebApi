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
using Metrics;
using TennisWithMe_WebApi.Services.Interfaces;
using TennisWithMe_WebApi.Helpers;

namespace TennisWithMe_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/PlayerRatings")]
    public class PlayerRatingsController : ApiController
    {
        private IPlayerRatingsService _playerRatingsService;
        private IMapper _mapperToModel;
        private IMapper _mapperToEntity;

        public PlayerRatingsController()
        {
            _playerRatingsService = new PlayerRatingsServiceImpl();
            _mapperToModel = new MapperConfiguration(cfg => cfg.CreateMap<PlayersRating, PlayersRatingViewModel>()).CreateMapper();
            _mapperToEntity = new MapperConfiguration(cfg => cfg.CreateMap<PlayersRatingViewModel, PlayersRating>()).CreateMapper();
        }

        public PlayerRatingsController(IPlayerRatingsService playerRatingsService)
        {
            _playerRatingsService = playerRatingsService;
            _mapperToModel = new MapperConfiguration(cfg => cfg.CreateMap<PlayersRating, PlayersRatingViewModel>()).CreateMapper();
            _mapperToEntity = new MapperConfiguration(cfg => cfg.CreateMap<PlayersRatingViewModel, PlayersRating>()).CreateMapper();
        }

        [HttpGet]
        [Route("")]
        [TimerAspect]
        public async Task<IHttpActionResult> GetPlayerRatingForFriendId(string friendId, string userID = null)
        {
            string appUserID = (userID == null) ? User.Identity.GetUserId() : userID;

            try
            {
                var playersRating = await _playerRatingsService.GetPlayersRatingForIdAndFriendId(appUserID, friendId);
                var playersRatingModel = _mapperToModel.Map<PlayersRatingViewModel>(playersRating);

                return Ok<PlayersRatingViewModel>(playersRatingModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("")]
        [TimerAspect]
        public async Task<IHttpActionResult> CreateOrUpdatePlayersRating(PlayersRatingViewModel model)
        {
            try
            {
                var playersRating = _mapperToEntity.Map<PlayersRating>(model);
                await _playerRatingsService.CreateOrUpdatePlayersRating(model, playersRating);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
