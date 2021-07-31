using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TitleFetcherAPI.Master.Models.RequestModels;
using TitleFetcherAPI.Master.Services;
using TitleFetcherAPI.Master.Services.Abstractions;

namespace TitleFetcherAPI.Master.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TitleController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IQueueManagerService _queueManager;
        private readonly ITitleStorageService _storage;

        public TitleController(IConfiguration config, IQueueManagerService queueManager, ITitleStorageService storage)
        {
            _config = config;
            _queueManager = queueManager;
            _storage = storage;
        }

        [HttpPost]
        [Route("send-titles")]
        public IActionResult SendTitles(TitleRequest req)
        {
            try
            {
                _queueManager.SendMessage(req.Urls);
                return Ok("Ok");
            } 
            catch (Exception ex) 
            {
                return BadRequest("Error");
            }                       
        }

        [HttpGet]
        [Route("get-recent-titles/{timeframe}")]
        public IActionResult GetRecentTitles(string timeframe)
        {
            return Ok(_storage.GetUrlTitlesForLastTimeFrame(timeframe));
        }
    }
}