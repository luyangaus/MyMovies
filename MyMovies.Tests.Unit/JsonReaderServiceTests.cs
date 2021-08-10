using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using MyMovies.Service;
using Xunit;

namespace MyMovies.Tests.Unit
{
    public class JsonReaderServiceTests
    {
        private readonly IJsonReaderService _readService;
        private readonly Mock<ILogger<JsonReaderService>> _loggerMock;

        public JsonReaderServiceTests()
        {
            _loggerMock = new Mock<ILogger<JsonReaderService>>();
            _readService = new JsonReaderService(_loggerMock.Object);
        }

        [Fact]
        public void ReadAPISourceInfo_With_Non_Exist_File_Retrun_Empty_List()
        {
            var nonExistFilePath = "notAPath";

            var result = _readService.ReadAPISourceInfo(nonExistFilePath);

            Assert.Empty(result);
            _loggerMock.Verify(logger => logger.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once());
        }

        [Fact]
        public void ReadAPISourceInfo_With_Non_Exist_File_Retrun_List()
        {
            var filePath = "APILists.json";

            var result = _readService.ReadAPISourceInfo(filePath);

            Assert.NotEmpty(result);
        }

    }
}
