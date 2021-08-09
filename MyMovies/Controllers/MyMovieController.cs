using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMovies.Models;
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
        [HttpGet("GetMovies")]
        public async Task<List<MovieDto>> GetMovies()
        {
            return new List<MovieDto>();
        }
    }
}
