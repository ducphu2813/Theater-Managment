﻿using Movie.Domain.Entity;
using Shared.Interfaces;

namespace Movie.Domain.Interface;

public interface IMovieRepository : IRepository<movies>
{
    Task<List<movies>> GetAllMovieAsyncById(List<string> movieIds);
    
    //hàm tìm movie nâng cao
    Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , List<string> name
        , List<string> director
        , List<string> actor
        , List<string> author
        , List<string> dub
        , List<string> subtitle
        , List<string> genres);
}