using MyMovies.Application;
using MyMovies.Controllers;
using MyMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyMovies.Tests.Unit
{
    public class MovieDetailsHelperTests
    {
        [Fact]
        public void MovieDetailsHelper_With_Empty_MovieList_Return_Empty()
        {
            var emptyList = new List<MovieDto>();

            var result = MovieDetailsHelper.GetCheapestMovie(emptyList);

            Assert.Empty(result);
        }

        [Fact]
        public void MovieDetailsHelper_With_Different_Movies()
        {
            var emptyList = new List<MovieDto> { 
                new MovieDto{ Title="M1", Price = 10 },
                new MovieDto{ Title="M2", Price = 15 }
            };

            var result = MovieDetailsHelper.GetCheapestMovie(emptyList);

            Assert.Equal(2,result.Count);
        }

        [Fact]
        public void MovieDetailsHelper_With_Same_Movie_Return_Cheaper()
        {
            var emptyList = new List<MovieDto> {
                new MovieDto{ Title="M1", Price = 10 },
                new MovieDto{ Title="M1", Price = 15 }
            };

            var result = MovieDetailsHelper.GetCheapestMovie(emptyList);

            Assert.Single(result);
            Assert.Equal(10, result.FirstOrDefault().Price);
        }

        [Fact]
        public void MovieDetailsHelper_With_Different_Movie_Return_Cheaper()
        {
            var emptyList = new List<MovieDto> {
                new MovieDto{ Title="M1", Price = 10 },
                new MovieDto{ Title="M1", Price = 15 },
                new MovieDto{ Title="M2", Price = 5},
                new MovieDto{ Title="M2", Price = 10},
                new MovieDto{ Title="M3", Price = 5}
            };

            var result = MovieDetailsHelper.GetCheapestMovie(emptyList);

            Assert.Equal(3, result.Count);
        }
    }
}
