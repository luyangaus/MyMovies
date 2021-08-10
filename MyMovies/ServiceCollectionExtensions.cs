using Microsoft.Extensions.DependencyInjection;
using MyMovies.Repository;
using MyMovies.Repository.Interface;

namespace MyMovies
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMoviesAPICommunicationRepository(this IServiceCollection services)
        {
            services.AddTransient<IMoviesAPICommunicationRepository, MoviesAPICommunicationRepository>();
        }
    }
}