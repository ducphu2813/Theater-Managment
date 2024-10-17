using MongoDB.Driver;
using ReservationService.Entity.Model;
using ReservationService.Repository.Interface;
using ReservationService.Repository.MongoDBRepo;
using ReservationService.Context;

namespace ReservationService.Repository;

public class SeatRepository : MongoDBRepository<Seat>, ISeatRepository
{
    public SeatRepository(MongoDBContext context) : base(context, "Seat")
    {
    }
    
    //hàm lấy danh sách Seat theo RoomNumber
    public async Task<List<Seat>> GetByRoomNumberAsync(string roomNumber)
    {
        var filter = Builders<Seat>.Filter.Eq("RoomNumber", roomNumber);
        return await _collection.Find(filter).ToListAsync();
    }
    
    //hàm lấy danh sách Seat theo danh sách id
    public async Task<List<Seat>> GetByIdsAsync(List<string> ids)
    {
        var filter = Builders<Seat>.Filter.In("Id", ids);
        return await _collection.Find(filter).ToListAsync();
    }
    
}