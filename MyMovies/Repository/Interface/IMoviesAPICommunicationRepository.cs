using MyMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMovies.Repository.Interface
{
    interface IMoviesAPICommunicationRepository
    {
        Task<List<MovieDto>> GetMovies(string fromSource);
    }
}
