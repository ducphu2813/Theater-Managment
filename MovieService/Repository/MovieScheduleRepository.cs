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
    
}