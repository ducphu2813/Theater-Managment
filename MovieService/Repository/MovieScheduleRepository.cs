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
    
}