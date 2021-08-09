using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using MyMovies.Models;
using Newtonsoft.Json;
using MyMovies.Repository.Interface;

namespace MyMovies.Repository
{
    public class MoviesAPICommunicationRepository: IMoviesAPICommunicationRepository
    {
        private readonly HttpClient _httpClient;
        private string _baseUrl;
        private string _secureHeader;
        private string _secureHeaderValue;

        public MoviesAPICommunicationRepository(string baseURL, string secureHeader, string secureHeaderValue)
        {
            _httpClient = CreateWorkaroundClient();
            _baseUrl = baseURL;
            _secureHeader = secureHeader;
            _secureHeaderValue = secureHeaderValue;
        }

        public async Task<List<MovieDto>> GetMoviesFromSource(string fromSource)
        {
            var url = $"{_baseUrl}/api/{fromSource}/movies";
            _httpClient.DefaultRequestHeaders.Add(_secureHeader, _secureHeaderValue);
            var response = await _httpClient.GetAsync(url);
            var responseString = await new StreamReader(await response.Content.ReadAsStreamAsync()).ReadToEndAsync();
            var result = JsonConvert.DeserializeObject<List<MovieDto>>(responseString);
            return result;
        }


        private HttpClient CreateWorkaroundClient()
        {
            SocketsHttpHandler handler = new SocketsHttpHandler
            {
                ConnectCallback = IPv4ConnectAsync
            };
            return new HttpClient(handler);

            static async ValueTask<Stream> IPv4ConnectAsync(SocketsHttpConnectionContext context, CancellationToken cancellationToken)
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.NoDelay = true;

                try
                {
                    await socket.ConnectAsync(context.DnsEndPoint, cancellationToken).ConfigureAwait(false);
                    return new NetworkStream(socket, ownsSocket: true);
                }
                catch
                {
                    socket.Dispose();
                    throw;
                }
            }
        }
    }
}
