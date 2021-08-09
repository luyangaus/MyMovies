using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyMovies.Models;
using MyMovies.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyMovieController : ControllerBase
    {
        private readonly IMoviesAPICommunicationRepository _moviesAPICommunicationRepository;
        private readonly ILogger<MyMovieController> _logger;

        public MyMovieController(IMoviesAPICommunicationRepository moviesAPICommunicationRepository, ILogger<MyMovieController> logger) :base()
        {
            _moviesAPICommunicationRepository = moviesAPICommunicationRepository;
            _logger = logger;
        }

        [HttpGet("GetMovies")]
        public async Task<List<MovieDto>> GetMovies()
        {
            var result = await _moviesAPICommunicationRepository.GetMoviesFromSource("cinemaworld");
            return new List<MovieDto>();
        }
    }
}
