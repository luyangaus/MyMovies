using System.Collections.Generic;
using MyMovies.Models;

namespace MyMovies.Service
{
    public interface IJsonReaderService
    {
        List<APISourceInfo> ReadAPISourceInfo(string fileName);
    }
}