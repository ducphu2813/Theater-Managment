using MovieService.Core.Entity.Model;

namespace MovieService.Core.Interfaces.Repository;

public interface IRoomRepository : IRepository<Room>
{

    Task<IEnumerable<Room>> AddList(List<Room> rooms);

}