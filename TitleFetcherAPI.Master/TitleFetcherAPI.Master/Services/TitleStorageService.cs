using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TitleFetcherAPI.Master.Models;
using TitleFetcherAPI.Master.Models.ResponseModels;
using TitleFetcherAPI.Master.Services.Abstractions;

namespace TitleFetcherAPI.Master.Services
{
    public class TitleStorageService : ITitleStorageService
    {
        private static ConcurrentBag<UrlTitle> urlTitles = new ConcurrentBag<UrlTitle>();

        private readonly IConfiguration _config;

        public TitleStorageService(IConfiguration config)
        {
            _config = config;
        }

        public void AddUrlTitle(UrlTitle ut)
        {
            urlTitles.Add(ut);
        }

        public IEnumerable<TitleResponse> GetUrlTitlesForLastTimeFrame(string timeframe)
        {
            var minutes = GetRequiredMinutes(timeframe);

            List<TitleResponse> recent = new List<TitleResponse>();

            DateTime validDate = DateTime.Now.AddMinutes(-minutes);
            foreach (var ut in urlTitles)
            {
                if (ut.DateAdded > validDate)
                    recent.Add(new TitleResponse { Title = ut.Title, Url = ut.Url });
            }

            return recent;
        }

        private int GetRequiredMinutes(string timeframe)
        {
            bool isNumeric = int.TryParse(timeframe, out int minutesNum) && !timeframe.Equals("-1");
            var defaultMins = _config.GetSection("Titles").GetValue<int>("DefaultTimeFrame");

            return isNumeric ? minutesNum : defaultMins;
        }
    }
}
