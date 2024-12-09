using MongoDB.Driver;
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
    
    //hàm kiểm tra xem phòng có tồn tại ko theo số phòng
    public async Task<bool> CheckExistRoomAsync(string roomNumber)
    {
        var result = await _collection.Find(x => x.RoomNumber == roomNumber).FirstOrDefaultAsync();
        return result != null;
    }
    
}