using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyMovies.Models;
using MyMovies.Repository.Interface;

namespace MyMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyMovieController : ControllerBase
    {
        private readonly ILogger<MyMovieController> _logger;
        private readonly IMoviesAPICommunicationRepository _moviesAPICommunicationRepository;

        public MyMovieController(IMoviesAPICommunicationRepository moviesAPICommunicationRepository,
            ILogger<MyMovieController> logger)
        {
            _moviesAPICommunicationRepository = moviesAPICommunicationRepository;
            _logger = logger;
        }

        [HttpGet("GetMovies")]
        public async Task<List<MovieDto>> GetMovies()
        {
            var allMovies = await _moviesAPICommunicationRepository.GetMoviesFromSource();
            var allMovieDetails = await _moviesAPICommunicationRepository.GetMovieById(allMovies);

            var movieWithLowestPrice =
                allMovieDetails
                    .GroupBy(x => x.Title)
                    .Select(x => new MovieDto
                    {
                        SiteName = x.FirstOrDefault().SiteName,
                        Title = x.Key,
                        Price = x.Min(x => x.Price),
                        Year = x.FirstOrDefault().Year,
                        Poster = x.FirstOrDefault().Poster,
                        ID = x.FirstOrDefault().ID
                    })
                    .ToList();
            return movieWithLowestPrice;
        }
    }
}