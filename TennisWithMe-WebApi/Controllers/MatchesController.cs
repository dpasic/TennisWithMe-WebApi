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
    [RoutePrefix("api/Matches")]
    public class MatchesController : ApiController
    {
        private IMatchesService _matchesService;
        private IMapper _mapperToMatchModel;
        private IMapper _mapperToMatch;

        public MatchesController()
        {
            _matchesService = new MatchesServiceDb();
            _mapperToMatchModel = new MapperConfiguration(cfg => cfg.CreateMap<Match, MatchViewModel>()).CreateMapper();
            _mapperToMatch = new MapperConfiguration(cfg => cfg.CreateMap<MatchViewModel, Match>()).CreateMapper();
        }

        public MatchesController(IMatchesService matchesService)
        {
            _matchesService = matchesService;
            _mapperToMatchModel = new MapperConfiguration(cfg => cfg.CreateMap<Match, MatchViewModel>()).CreateMapper();
            _mapperToMatch = new MapperConfiguration(cfg => cfg.CreateMap<MatchViewModel, Match>()).CreateMapper();
        }

        [HttpGet]
        [Route("Active")]
        [TimerAspect]
        public async Task<IHttpActionResult> GetActiveMatches(string userID = null)
        {
            string appUserID = (userID == null) ? User.Identity.GetUserId() : userID;

            try
            {
                var activeMatches = await _matchesService.GetActiveMatchesForId(appUserID);
                var matchesModels = _mapperToMatchModel.Map<List<MatchViewModel>>(activeMatches);

                return Ok<List<MatchViewModel>>(matchesModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Requested")]
        [TimerAspect]
        public async Task<IHttpActionResult> GetRequestedMatches(string userID = null)
        {
            string appUserID = (userID == null) ? User.Identity.GetUserId() : userID;

            try
            {
                var requestedMatches = await _matchesService.GetRequestedMatchesForId(appUserID);
                var matchesModels = _mapperToMatchModel.Map<List<MatchViewModel>>(requestedMatches);

                foreach (var match in matchesModels.Where(x => x.PlayerTwoId == appUserID))
                {
                    match.IsMatchReceived = true;
                }

                return Ok<List<MatchViewModel>>(matchesModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("RequestMatch")]
        [TimerAspect]
        public async Task<IHttpActionResult> RequestMatch(MatchViewModel model, string userID = null)
        {
            string appUserID = (userID == null) ? User.Identity.GetUserId() : userID;
            model.PlayerOneId = (model.PlayerOneId == null) ? appUserID : model.PlayerOneId;

            try
            {
                var match = _mapperToMatch.Map<Match>(model);
                await _matchesService.RequestMatch(match);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("ConfirmMatch")]
        [TimerAspect]
        public async Task<IHttpActionResult> ConfirmMatch(MatchViewModel model, string userID = null)
        {
            string appUserID = (userID == null) ? User.Identity.GetUserId() : userID;
            model.PlayerOneId = (model.PlayerOneId == null) ? appUserID : model.PlayerOneId;

            try
            {
                var match = _mapperToMatch.Map<Match>(model);
                await _matchesService.ConfirmMatch(match);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateMatch")]
        [TimerAspect]
        public async Task<IHttpActionResult> UpdateMatch(MatchViewModel model)
        {
            try
            {
                await _matchesService.UpdateMatch(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
