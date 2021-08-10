using MyMovies.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyMovies.Infrastructure
{
    public class HttpHandler:IHttpHandler
    {
        public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string,string> headerPairs)
        {
            var _client = new HttpClient();
            foreach (var headerPair in headerPairs)
            {
                _client.DefaultRequestHeaders.Add(headerPair.Key, headerPair.Value);
            }
            
            return await _client.GetAsync(url);
        }

    }
}
