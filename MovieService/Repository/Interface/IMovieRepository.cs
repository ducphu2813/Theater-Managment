using MovieService.DTO;
using MovieService.Entity.Model;
using MovieService.Repository.MongoDBRepo;

namespace MovieService.Repository.Interface;

public interface IMovieRepository : IRepository<movies>
{
    Task<List<movies>> GetAllMovieAsyncById(List<string> movieIds);
}