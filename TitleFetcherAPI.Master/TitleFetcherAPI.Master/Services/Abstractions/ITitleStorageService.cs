using System.Collections.Generic;
using TitleFetcherAPI.Master.Models;
using TitleFetcherAPI.Master.Models.ResponseModels;

namespace TitleFetcherAPI.Master.Services.Abstractions
{
    public interface ITitleStorageService
    {
        void AddUrlTitle(UrlTitle ut);
        IEnumerable<TitleResponse> GetUrlTitlesForLastTimeFrame(string timeframe);
    }
}
