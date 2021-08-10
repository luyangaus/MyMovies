using System.Collections.Generic;
using System.Threading.Tasks;
using MyMovies.Models;

namespace MyMovies.Repository.Interface
{
    public interface IMoviesAPICommunicationRepository
    {
        Task<List<MovieDto>> GetMoviesFromSource();
        Task<List<MovieDto>> GetMovieById(List<MovieDto> request);
    }
}