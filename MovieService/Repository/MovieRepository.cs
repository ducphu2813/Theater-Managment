using Microsoft.VisualBasic;
using MongoDB.Driver;
using MovieService.Context;
using MovieService.DTO;
using MovieService.Entity.Model;
using MovieService.Repository.Interface;
using MovieService.Repository.MongoDBRepo;

namespace MovieService.Repository;

public class MovieRepository : MongoDBRepository<movies>, IMovieRepository
{
    public MovieRepository(MongoDBContext context) : base(context, "movies")
    {

    }
    
    //hàm này dê lấy tất cả dto, phục vụ cho bên movie schedule
    public async Task<List<movies>> GetAllMovieAsyncById(List<string> movieIds)
    {
        var filter = Builders<movies>.Filter.In(x => x.Id, movieIds);
        var movies = await _collection.Find(filter).ToListAsync();
        return movies;
    }
    
    
    
    
    
    
    
    
    
    
    
}