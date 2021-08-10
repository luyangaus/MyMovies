using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyMovies.Models;
using MyMovies.Repository.Interface;
using MyMovies.Service;
using Newtonsoft.Json;

namespace MyMovies.Repository
{
    public class MoviesAPICommunicationRepository : IMoviesAPICommunicationRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly IJsonReaderService _jsonReaderService;

        public MoviesAPICommunicationRepository(IJsonReaderService jsonReaderService, IConfiguration config,
            ILogger<MoviesAPICommunicationRepository> logger)
        {
            _jsonReaderService = jsonReaderService;
            _config = config;
            _logger = logger;
        }

        public async Task<List<MovieDto>> GetMoviesFromSource()
        {
            var apiSourceFilePath = _config.GetSection("APISourceFile").Get<string>();
            var aPIs = _jsonReaderService.ReadAPISourceInfo(apiSourceFilePath);
            var getMoviesApi = aPIs.Where(api => api.APIUsage == "GetAllMovies");
            var result = new List<MovieDto>();

            Parallel.ForEach(getMoviesApi, new ParallelOptions {MaxDegreeOfParallelism = 2}, api =>
            {
                try
                {
                    var httpClient = CreateWorkaroundClient();
                    httpClient.DefaultRequestHeaders.Add(api.AccessHeader, api.AccessHeaderValue);
                    var response = httpClient.GetAsync(api.BaseURL).Result;
                    var responseString = new StreamReader(response.Content.ReadAsStreamAsync().Result).ReadToEndAsync()
                        .Result;
                    var movies = JsonConvert.DeserializeObject<MovieContainer>(responseString).Movies;
                    movies.ForEach(m => m.SiteName = api.SiteName);
                    result.AddRange(movies);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            });

            return result;
        }

        public async Task<List<MovieDto>> GetMovieById(List<MovieDto> request)
        {
            var apiSourceFilePath = _config.GetSection("APISourceFile").Get<string>();
            var aPIs = _jsonReaderService.ReadAPISourceInfo(apiSourceFilePath);
            var getMovieApi = aPIs.Where(api => api.APIUsage == "GetMovieById");
            var result = new List<MovieDto>();

            Parallel.ForEach(request, new ParallelOptions {MaxDegreeOfParallelism = 10}, req =>
            {
                try
                {
                    var api = getMovieApi.Where(api => api.SiteName == req.SiteName).FirstOrDefault();
                    var httpClient = CreateWorkaroundClient();
                    httpClient.DefaultRequestHeaders.Add(api.AccessHeader, api.AccessHeaderValue);
                    var response = httpClient.GetAsync($"{api.BaseURL}/{req.ID}").Result;
                    var responseString = new StreamReader(response.Content.ReadAsStreamAsync().Result).ReadToEndAsync()
                        .Result;
                    var movie = JsonConvert.DeserializeObject<MovieDto>(responseString);
                    movie.SiteName = api.SiteName;
                    result.Add(movie);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            });

            return result;
        }

        private HttpClient CreateWorkaroundClient()
        {
            var handler = new SocketsHttpHandler
            {
                ConnectCallback = IPv4ConnectAsync
            };
            return new HttpClient(handler);

            static async ValueTask<Stream> IPv4ConnectAsync(SocketsHttpConnectionContext context,
                CancellationToken cancellationToken)
            {
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.NoDelay = true;

                try
                {
                    await socket.ConnectAsync(context.DnsEndPoint, cancellationToken).ConfigureAwait(false);
                    return new NetworkStream(socket, true);
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