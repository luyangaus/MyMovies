using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using MyMovies.Models;
using Newtonsoft.Json;

namespace MyMovies.Service
{
    public class JsonReaderService : IJsonReaderService
    {
        private readonly ILogger<JsonReaderService> _logger;

        public JsonReaderService(ILogger<JsonReaderService> logger)
        {
            _logger = logger;
        }

        public List<APISourceInfo> ReadAPISourceInfo(string fileName)
        {
            try
            {
                var fileString = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<APISourceContainer>(fileString);

                return data.APISourceInfos;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<APISourceInfo>();
            }
        }
    }
}