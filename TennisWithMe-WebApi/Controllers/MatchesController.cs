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
        private IMapper _mapperToMatchModel;
        private IMapper _mapperToMatch;

        public MatchesController()
        {
            _matchesService = MatchesService.Instance;
            _mapperToMatchModel = new MapperConfiguration(cfg => cfg.CreateMap<Match, MatchViewModel>()).CreateMapper();
            _mapperToMatch = new MapperConfiguration(cfg => cfg.CreateMap<MatchViewModel, Match>()).CreateMapper();
        }

        [HttpGet]
        [Route("Active")]
        public async Task<IHttpActionResult> GetActiveMatches()
        {
            string appUserID = User.Identity.GetUserId();

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
        public async Task<IHttpActionResult> GetRequestedMatches()
        {
            string appUserID = User.Identity.GetUserId();

            try
            {
                var requestedMatches = await _matchesService.GetRequestedMatchesForId(appUserID);
                var matchesModels = _mapperToMatchModel.Map<List<MatchViewModel>>(requestedMatches);

                return Ok<List<MatchViewModel>>(matchesModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("RequestMatch")]
        public async Task<IHttpActionResult> RequestMatch(MatchViewModel model)
        {
            string appUserID = User.Identity.GetUserId();
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
        public async Task<IHttpActionResult> ConfirmMatch(MatchViewModel model)
        {
            string appUserID = User.Identity.GetUserId();
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
    }
}
