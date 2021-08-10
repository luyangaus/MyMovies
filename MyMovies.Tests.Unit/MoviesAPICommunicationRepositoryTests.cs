using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MyMovies.Interface;
using MyMovies.Models;
using MyMovies.Repository;
using MyMovies.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace MyMovies.Tests.Unit
{
    public class MoviesAPICommunicationRepositoryTests
    {

        private readonly IConfiguration _config;
        private readonly Mock<ILogger<MoviesAPICommunicationRepository>> _loggerMock;
        private readonly Mock<IJsonReaderService> _jsonReaderServiceMock;
        private readonly Mock<IHttpHandler> _httpClientMock;

        public MoviesAPICommunicationRepositoryTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"APISourceFile", "APILists.json"},
            };

            _config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            _loggerMock = new Mock<ILogger<MoviesAPICommunicationRepository>>();
            _jsonReaderServiceMock = new Mock<IJsonReaderService>();
            _httpClientMock = new Mock<IHttpHandler>();
        }

        [Fact]
        public void GetMoviesFromSourceTest_With_No_API_Return_Empty_List()
        {
            _jsonReaderServiceMock.Setup(j => j.ReadAPISourceInfo(It.IsAny<string>())).Returns(new List<APISourceInfo>());
            var repository = new MoviesAPICommunicationRepository(_jsonReaderServiceMock.Object, _config, _loggerMock.Object, _httpClientMock.Object);

            var result = repository.GetMoviesFromSource().Result;

            Assert.Empty(result);
        }

        [Fact]
        public void GetMoviesFromSourceTest_With_APIs_Return_List()
        {
            _jsonReaderServiceMock.Setup(j => j.ReadAPISourceInfo(It.IsAny<string>())).Returns(new List<APISourceInfo> { new APISourceInfo { AccessHeader = "header", AccessHeaderValue = "Value", APIUsage = "GetAllMovies", BaseURL = "url" } });
            var response = " {\r\n  \"Movies\": [\r\n    {\r\n      \"Title\": \"Star Wars: Episode IV - A New Hope\",\r\n      \"Year\": \"1977\",\r\n      \"ID\": \"cw0076759\",\r\n      \"Type\": \"movie\",\r\n      \"Poster\": \"https://m.media-amazon.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"\r\n    },\r\n    {\r\n      \"Title\": \"Star Wars: Episode V - The Empire Strikes Back\",\r\n      \"Year\": \"1980\",\r\n      \"ID\": \"cw0080684\",\r\n      \"Type\": \"movie\",\r\n      \"Poster\": \"https://m.media-amazon.com/images/M/MV5BMjE2MzQwMTgxN15BMl5BanBnXkFtZTcwMDQzNjk2OQ@@._V1_SX300.jpg\"\r\n    },\r\n    {\r\n      \"Title\": \"Star Wars: Episode VI - Return of the Jedi\",\r\n      \"Year\": \"1983\",\r\n      \"ID\": \"cw0086190\",\r\n      \"Type\": \"movie\",\r\n      \"Poster\": \"https://m.media-amazon.com/images/M/MV5BMTQ0MzI1NjYwOF5BMl5BanBnXkFtZTgwODU3NDU2MTE@._V1._CR93,97,1209,1861_SX89_AL_.jpg_V1_SX300.jpg\"\r\n    },\r\n    {\r\n      \"Title\": \"Star Wars: The Force Awakens\",\r\n      \"Year\": \"2015\",\r\n      \"ID\": \"cw2488496\",\r\n      \"Type\": \"movie\",\r\n      \"Poster\": \"https://m.media-amazon.com/images/M/MV5BOTAzODEzNDAzMl5BMl5BanBnXkFtZTgwMDU1MTgzNzE@._V1_SX300.jpg\"\r\n    },\r\n    {\r\n      \"Title\": \"Star Wars: Episode I - The Phantom Menace\",\r\n      \"Year\": \"1999\",\r\n      \"ID\": \"cw0120915\",\r\n      \"Type\": \"movie\",\r\n      \"Poster\": \"https://m.media-amazon.com/images/M/MV5BMTQ4NjEwNDA2Nl5BMl5BanBnXkFtZTcwNDUyNDQzNw@@._V1_SX300.jpg\"\r\n    },\r\n    {\r\n      \"Title\": \"Star Wars: Episode III - Revenge of the Sith\",\r\n      \"Year\": \"2005\",\r\n      \"ID\": \"cw0121766\",\r\n      \"Type\": \"movie\",\r\n      \"Poster\": \"https://m.media-amazon.com/images/M/MV5BNTc4MTc3NTQ5OF5BMl5BanBnXkFtZTcwOTg0NjI4NA@@._V1_SX300.jpg\"\r\n    },\r\n    {\r\n      \"Title\": \"Star Wars: Episode II - Attack of the Clones\",\r\n      \"Year\": \"2002\",\r\n      \"ID\": \"cw0121765\",\r\n      \"Type\": \"movie\",\r\n      \"Poster\": \"https://m.media-amazon.com/images/M/MV5BMTY5MjI5NTIwNl5BMl5BanBnXkFtZTYwMTM1Njg2._V1_SX300.jpg\"\r\n    }\r\n  ]\r\n}\r\n";

            _httpClientMock.Setup(h => h.GetAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ReturnsAsync(new HttpResponseMessage { Content = new StringContent(response) });
            
            var repository = new MoviesAPICommunicationRepository(_jsonReaderServiceMock.Object, _config, _loggerMock.Object, _httpClientMock.Object);

            var result = repository.GetMoviesFromSource().Result;

            Assert.Equal(7,result.Count);
        }

        [Fact]
        public void GetMovieById_With_APIs_Return_List()
        {
            _jsonReaderServiceMock.Setup(j => j.ReadAPISourceInfo(It.IsAny<string>())).Returns(new List<APISourceInfo> { new APISourceInfo { AccessHeader = "header", AccessHeaderValue = "Value", APIUsage = "GetMovieById", BaseURL = "url",SiteName = "site" } });
            var response = "{\n  \"Title\": \"Star Wars: Episode VI - Return of the Jedi\",\n  \"Year\": \"1983\",\n  \"Rated\": \"PG\",\n  \"Released\": \"25 May 1983\",\n  \"Runtime\": \"131 min\",\n  \"Genre\": \"Action, Adventure, Fantasy\",\n  \"Director\": \"Richard Marquand\",\n  \"Writer\": \"Lawrence Kasdan (screenplay), George Lucas (screenplay), George Lucas (story by)\",\n  \"Actors\": \"Mark Hamill, Harrison Ford, Carrie Fisher, Billy Dee Williams\",\n  \"Plot\": \"After rescuing Han Solo from the palace of Jabba the Hutt, the rebels attempt to destroy the second Death Star, while Luke struggles to make Vader return from the dark side of the Force.\",\n  \"Language\": \"English\",\n  \"Country\": \"USA\",\n  \"Awards\": \"Nominated for 4 Oscars. Another 18 wins & 16 nominations.\",\n  \"Poster\": \"https://m.media-amazon.com/images/M/MV5BMTQ0MzI1NjYwOF5BMl5BanBnXkFtZTgwODU3NDU2MTE@._V1._CR93,97,1209,1861_SX89_AL_.jpg_V1_SX300.jpg\",\n  \"Metascore\": \"53\",\n  \"Rating\": \"8.4\",\n  \"Votes\": \"686,479\",\n  \"ID\": \"cw0086190\",\n  \"Type\": \"movie\",\n  \"Price\": \"253.5\"\n}";

            _httpClientMock.Setup(h => h.GetAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ReturnsAsync(new HttpResponseMessage { Content = new StringContent(response) });

            var repository = new MoviesAPICommunicationRepository(_jsonReaderServiceMock.Object, _config, _loggerMock.Object, _httpClientMock.Object);

            var result = repository.GetMovieById(new List<MovieDto>{ new MovieDto { SiteName = "site" } }).Result;
            var movieDto = result.FirstOrDefault();
            
            Assert.Equal("Star Wars: Episode VI - Return of the Jedi",movieDto.Title);
            Assert.Equal("site", movieDto.SiteName);
            Assert.Equal("1983", movieDto.Year);
            Assert.Equal("PG", movieDto.Rated);
            Assert.Equal("25 May 1983", movieDto.Released);
            Assert.Equal("131 min", movieDto.Runtime);
            Assert.Equal("Action, Adventure, Fantasy", movieDto.Genre);
            Assert.Equal("Richard Marquand", movieDto.Director);
            Assert.Equal("Lawrence Kasdan (screenplay), George Lucas (screenplay), George Lucas (story by)", movieDto.Writer);
            Assert.Equal("Mark Hamill, Harrison Ford, Carrie Fisher, Billy Dee Williams", movieDto.Actors);
            Assert.Equal("After rescuing Han Solo from the palace of Jabba the Hutt, the rebels attempt to destroy the second Death Star, while Luke struggles to make Vader return from the dark side of the Force.", movieDto.Plot);
            Assert.Equal("English", movieDto.Language);
            Assert.Equal("USA", movieDto.Country);
            Assert.Equal("Nominated for 4 Oscars. Another 18 wins & 16 nominations.", movieDto.Awards);
            Assert.Equal("https://m.media-amazon.com/images/M/MV5BMTQ0MzI1NjYwOF5BMl5BanBnXkFtZTgwODU3NDU2MTE@._V1._CR93,97,1209,1861_SX89_AL_.jpg_V1_SX300.jpg", movieDto.Poster);
            Assert.Equal("53", movieDto.Metascore);
            Assert.Equal("8.4", movieDto.Rating);
            Assert.Equal("686,479", movieDto.Votes);
            Assert.Equal("cw0086190", movieDto.ID);
            Assert.Equal("movie", movieDto.Type);
            Assert.Equal(253.5m, movieDto.Price);
        }
    }
}
