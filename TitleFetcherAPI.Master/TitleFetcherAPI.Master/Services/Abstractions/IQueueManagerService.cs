using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TitleFetcherAPI.Master.Services
{
    public interface IQueueManagerService
    {
        void SendMessage(List<string> urls);
    }
}
