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
using MyMovies.Interface;
using MyMovies.Service;
using Newtonsoft.Json;

namespace MyMovies.Repository
{
    public class MoviesAPICommunicationRepository : IMoviesAPICommunicationRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MoviesAPICommunicationRepository> _logger;
        private readonly IJsonReaderService _jsonReaderService;
        private readonly IHttpHandler _httpClient;

        public MoviesAPICommunicationRepository(IJsonReaderService jsonReaderService, IConfiguration config,
            ILogger<MoviesAPICommunicationRepository> logger, IHttpHandler httpClient)
        {
            _jsonReaderService = jsonReaderService;
            _config = config;
            _logger = logger;
            _httpClient = httpClient;
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
                    var headerPair = new Dictionary<string, string>();
                    headerPair[api.AccessHeader] = api.AccessHeaderValue;
                    var response = _httpClient.GetAsync(api.BaseURL,headerPair).Result;
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
            var getMovieApi = aPIs.Where(api => api.APIUsage == nameof(GetMovieById));
            var result = new List<MovieDto>();

            Parallel.ForEach(request, new ParallelOptions {MaxDegreeOfParallelism = 10}, req =>
            {
                try
                {
                    var api = getMovieApi.Where(api => api.SiteName == req.SiteName).FirstOrDefault();
                    var headerPair = new Dictionary<string, string>();
                    headerPair[api.AccessHeader] = api.AccessHeaderValue;
                    var response = _httpClient.GetAsync(api.BaseURL+"/"+ req.ID, headerPair).Result;
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
    }
}