
using MovieService.Core.Entity.Model;
using MovieService.Core.Interfaces.Repository;
using MovieService.Infrastructure.Persistence.Context;

namespace MovieService.Infrastructure.Persistence.Repositories;

public class RoomRepository : MongoDBRepository<Room>, IRoomRepository
{
    public RoomRepository(MongoDBContext context) : base(context, "Room")
    {
    }
    
    public async Task<IEnumerable<Room>> AddList(List<Room> rooms)
    {
        await _collection.InsertManyAsync(rooms);
        return rooms;
    }
    
}