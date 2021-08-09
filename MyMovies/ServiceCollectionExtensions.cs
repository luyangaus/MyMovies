using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyMovies.Repository;
using MyMovies.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMovies
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMoviesAPICommunicationRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMoviesAPICommunicationRepository>(s =>
                new MoviesAPICommunicationRepository(configuration["MovieAPIURL"],
                    configuration["MovieHeader"], configuration["MovieHeaderValue"]));
        }
    }
}
