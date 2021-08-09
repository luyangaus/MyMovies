using MyMovies.Models;
using System.Collections.Generic;

namespace MyMovies.Service
{
    public interface IJsonReaderService
    {
        List<APISourceInfo> ReadAPISourceInfo(string fileName);
    }
}