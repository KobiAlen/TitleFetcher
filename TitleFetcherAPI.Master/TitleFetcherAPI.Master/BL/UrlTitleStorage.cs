using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TitleFetcherAPI.Master.Models;
using TitleFetcherAPI.Master.Models.ResponseModels;

namespace TitleFetcherAPI.Master.BL
{
    public static class UrlTitleStorage
    {
        private static List<UrlTitle> urlTitles = new List<UrlTitle>();

        /// <summary>
        /// Add a new UrlTitle to the list
        /// </summary>
        /// <param name="ut"></param>
        public static void AddUrlTitle(UrlTitle ut)
        {
            urlTitles.Add(ut);
        }

        /// <summary>
        /// Iterate through the urlTitles list and return a new list with entries only from said timeframe
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns>Returns a list of TitleResponse that can be shown to the user</returns>
        public static List<TitleResponse> GetUrlTitlesForLastTimeFrame(int minutes)
        {
            List<TitleResponse> recent = new List<TitleResponse>();

            DateTime validDate = DateTime.Now.AddMinutes(-minutes);
            foreach(var ut in urlTitles)
            {
                if (ut.DateAdded > validDate)
                    recent.Add(new TitleResponse { Title = ut.Title, Url = ut.Url });
            }

            return recent;
        }
    }
}
