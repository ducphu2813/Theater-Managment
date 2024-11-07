using MovieService.DTO;
using MovieService.Entity.Model;

namespace MovieService.Service.Interface;

public interface IMovieService
{
    Task<Dictionary<string, object>> GetAllAsync(int page, int limit);
    Task<movies> GetByIdAsync(string id);
    Task<movies> AddAsync(MovieDTO movieDto);
    Task<movies> UpdateAsync(string id, MovieDTO movieDto);
    Task<bool> RemoveAsync(string id);
    // Task<List<movies>> GetAllMovieAsyncById(List<string> movieIds);
    
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