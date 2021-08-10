using Microsoft.Extensions.DependencyInjection;
using MyMovies.Infrastructure;
using MyMovies.Repository;
using MyMovies.Interface;
using System;
using System.Net.Http;

namespace MyMovies
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMoviesAPICommunicationRepository(this IServiceCollection services)
        {
            services.AddTransient<IMoviesAPICommunicationRepository, MoviesAPICommunicationRepository>();
            services.AddTransient<IHttpHandler, HttpHandler>();
        }
    }
}