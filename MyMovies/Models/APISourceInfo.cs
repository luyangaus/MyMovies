using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMovies.Models
{
    public class APISourceInfo
    {
        public string SiteName { get; set; }
        public string BaseURL { get; set; }
        public string AccessHeader { get; set; }
        public string AccessHeaderValue { get; set; }
        public string APIUsage { get; set; }
    }

    public class APISourceContainer
    {
        public List<APISourceInfo> APISourceInfos { get; set; }
    }
}
