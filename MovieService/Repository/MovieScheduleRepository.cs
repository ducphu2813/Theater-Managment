using MongoDB.Driver;
using MovieService.Context;
using MovieService.Entity.Model;
using MovieService.Repository.Interface;
using MovieService.Repository.MongoDBRepo;

namespace MovieService.Repository;

public class MovieScheduleRepository : MongoDBRepository<MovieSchedule>, IMovieScheduleRepository
{
    public MovieScheduleRepository(MongoDBContext context) : base(context, "MovieSchedule")
    {
        
    }
    
    //hàm thêm 1 danh sách movie schedule
    public async Task<List<MovieSchedule>> AddListAsync(List<MovieSchedule> movieSchedules)
    {
        await _collection.InsertManyAsync(movieSchedules);
        return movieSchedules;
    }
    
    // lấy lịch chiếu theo id phim
    public async Task<List<MovieSchedule>> GetByMovieIdAsync(string movieId)
    {
        var filter = Builders<MovieSchedule>.Filter.Eq("MovieId", movieId);
        return await _collection.Find(filter).ToListAsync();
    }
    
}