using MovieService.Entity.Model;
using MovieService.Repository.MongoDBRepo;

namespace MovieService.Repository.Interface;

public interface IMovieScheduleRepository : IRepository<MovieSchedule>
{
    Task<List<MovieSchedule>> AddListAsync(List<MovieSchedule> movieSchedules);
}