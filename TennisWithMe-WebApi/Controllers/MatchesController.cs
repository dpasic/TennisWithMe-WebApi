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
    }
}
