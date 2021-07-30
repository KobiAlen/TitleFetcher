using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TitleFetcherAPI.Master.BL;
using TitleFetcherAPI.Master.Models.RequestModels;
using TitleFetcherAPI.Master.Services;

namespace TitleFetcherAPI.Master.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TitleController : ControllerBase
    {
        private readonly IConfiguration _config;
        private static IQueueManagerService _queueManager;      
        
        public TitleController(IConfiguration config, IQueueManagerService queueManager)
        {
            _config = config;
            _queueManager = queueManager;
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
        [Route("get-recent-titles/{minutes}")]
        public IActionResult GetRecentTitles(string minutes)
        {
            bool isNumeric = int.TryParse(minutes, out int minutesNum) && !minutes.Equals("-1");
            var defaultMins = _config.GetSection("Titles").GetValue<int>("DefaultTimeFrame");

            return Ok(UrlTitleStorage.GetUrlTitlesForLastTimeFrame(isNumeric ? minutesNum : defaultMins));
        }
    }
}