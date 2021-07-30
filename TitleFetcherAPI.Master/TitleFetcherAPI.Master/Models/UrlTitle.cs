using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TitleFetcherAPI.Master.Models
{
    public class UrlTitle
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
