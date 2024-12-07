using Movie.Domain.Entity;
using Movie.Domain.Interface;
using Shared.Context;
using Shared.Repository;

namespace Movie.Infrastructure.Repository;

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