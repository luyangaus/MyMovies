using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyMovies.Repository;
using MyMovies.Repository.Interface;
using MyMovies.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMovies
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMoviesAPICommunicationRepository(this IServiceCollection services)
        {
            services.AddTransient<IMoviesAPICommunicationRepository,MoviesAPICommunicationRepository>();
        }
    }
}
