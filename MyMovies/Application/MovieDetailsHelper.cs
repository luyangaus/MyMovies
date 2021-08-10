using MyMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMovies.Application
{
    public static class MovieDetailsHelper
    {
        public static List<MovieDto> GetCheapestMovie(List<MovieDto> movieDtos)
        {
            var movieWithLowestPrice =
                movieDtos
                    .GroupBy(x => x.Title)
                    .Select(x => new MovieDto
                    {
                        SiteName = x.FirstOrDefault().SiteName,
                        Title = x.Key,
                        Price = x.Min(x => x.Price),
                        Year = x.FirstOrDefault().Year,
                        Poster = x.FirstOrDefault().Poster,
                        ID = x.FirstOrDefault().ID
                    })
                    .ToList();

            return movieWithLowestPrice;
        }
    }
}
