using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TitleFetcher.Slave.TitleFetcher.Abstraction
{
    public interface ITitleFetcherService
    {
        Task<string> GetSiteTitle(string url);
    }
}
