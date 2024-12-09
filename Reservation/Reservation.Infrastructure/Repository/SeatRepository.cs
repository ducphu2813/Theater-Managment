using MongoDB.Driver;
using Reservation.Domain.Entity;
using Reservation.Domain.Interfaces;
using Shared.Context;
using Shared.Repository;

namespace Reservation.Infrastructure.Repository;

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
    
    //thêm danh sách nhiều ghế
    public async Task<List<Seat>> AddListAsync(List<Seat> seats)
    {
        await _collection.InsertManyAsync(seats);
        return seats;
    }
    
    //update nhiều seat
    public async Task<List<Seat>> UpdateListAsync(List<Seat> seats)
    {
        var bulkOps = new List<WriteModel<Seat>>();

        foreach (var seat in seats)
        {
            var filter = Builders<Seat>.Filter.Eq(s => s.Id, seat.Id);
            var update = Builders<Seat>.Update
                .Set(s => s.RoomNumber, seat.RoomNumber)
                .Set(s => s.Row, seat.Row)
                .Set(s => s.Column, seat.Column)
                .Set(s => s.SeatType, seat.SeatType);

            var updateOne = new UpdateOneModel<Seat>(filter, update);
            bulkOps.Add(updateOne);
        }

        if (bulkOps.Count > 0)
        {
            await _collection.BulkWriteAsync(bulkOps);
        }

        return seats;
    }
    
    //xóa ghế theo số phòng
    public async Task<bool> RemoveByRoomNumberAsync(string roomNumber)
    {
        var filter = Builders<Seat>.Filter.Eq("RoomNumber", roomNumber);
        var result = await _collection.DeleteManyAsync(filter);
        return result.DeletedCount > 0;
    }
    
}