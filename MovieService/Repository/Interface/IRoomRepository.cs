using MovieService.Entity.Model;
using MovieService.Repository.MongoDBRepo;

namespace MovieService.Repository.Interface;

public interface IRoomRepository : IRepository<Room>
{

    Task<IEnumerable<Room>> AddList(List<Room> rooms);

}