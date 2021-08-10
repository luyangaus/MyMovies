using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyMovies.Application;
using MyMovies.Models;
using MyMovies.Interface;

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
            var movieWithLowestPrice = MovieDetailsHelper.GetCheapestMovie(allMovieDetails);

            return movieWithLowestPrice;
        }
    }
}