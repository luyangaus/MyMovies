using MyMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMovies.Repository.Interface
{
    public interface IMoviesAPICommunicationRepository
    {
        Task<List<MovieDto>> GetMoviesFromSource(string fromSource);
    }
}
