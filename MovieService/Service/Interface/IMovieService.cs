using MovieService.DTO;
using MovieService.Entity.Model;

namespace MovieService.Service.Interface;

public interface IMovieService
{
    Task<IEnumerable<movies>> GetAllAsync();
    Task<movies> GetByIdAsync(string id);
    Task<movies> AddAsync(MovieDTO movieDto);
    Task<movies> UpdateAsync(string id, MovieDTO movieDto);
    Task<bool> RemoveAsync(string id);
    // Task<List<movies>> GetAllMovieAsyncById(List<string> movieIds);
}