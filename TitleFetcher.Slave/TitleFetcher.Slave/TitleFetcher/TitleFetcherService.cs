using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TitleFetcher.Slave.TitleFetcher.Abstraction
{
    public class TitleFetcherService : ITitleFetcherService
    {
        public async Task<string> GetSiteTitle(string url)
        {
            using var client = new HttpClient();

            if (!url.Substring(0, 4).Equals("http"))
                url = "http://" + url;

            try
            {
                var result = await client.GetAsync(url);
                var htmlString = await result.Content.ReadAsStringAsync();

                Match m = Regex.Match(htmlString, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>");

                if (m.Success)
                    return m.Groups[1].Value;
                else
                    return "";
            } catch (Exception ex)
            {
                return "";
            }            
        }
    }
}
