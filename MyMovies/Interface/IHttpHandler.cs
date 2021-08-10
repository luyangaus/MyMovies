using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyMovies.Interface
{
    public interface IHttpHandler
    {
        Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> headerPairs);
    }
}
