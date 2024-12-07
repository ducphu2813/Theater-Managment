using MongoDB.Driver;
using Reservation.Domain.Entity;
using Reservation.Domain.Interfaces;
using Shared.Context;
using Shared.Repository;

namespace Reservation.Infrastructure.Repository;

public class TicketRepository : MongoDBRepository<Ticket>, ITicketRepository
{
    public TicketRepository(MongoDBContext context) : base(context, "Ticket")
    {
    }
    
    //lấy danh sách vé theo id lịch chiếu
    public async Task<List<Ticket>> GetByScheduleIdAsync(string scheduleId)
    {
        var filter = Builders<Ticket>.Filter.Eq("MovieScheduleId", scheduleId);
        return await _collection.Find(filter).ToListAsync();
    }
    
    //lấy vé theo user id
    public async Task<List<Ticket>> GetByUserIdAsync(string userId)
    {
        var filter = Builders<Ticket>.Filter.Eq("UserId", userId);
        return await _collection.Find(filter).ToListAsync();
    }
    
    //hàm xóa tất cả vé
    public async Task<bool> RemoveAllAsync()
    {
        await _collection.DeleteManyAsync(Builders<Ticket>.Filter.Empty);
        return true;
    }
    
    //hàm tìm bằng schelude id và seat id và chỉ tìm những vé có Status là "processed"
    public async Task<List<Ticket>> GetByScheduleIdAndSeatIdAsync(string scheduleId, List<string> seatIds)
    {
        var filter = Builders<Ticket>.Filter.Eq("MovieScheduleId", scheduleId) &
                     Builders<Ticket>.Filter.In("SeatId", seatIds);
        
        return await _collection.Find(filter).ToListAsync();
    }
    
    //hàm tìm ticket nâng cao
    public async Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , string userId
        , List<string> scheduleId
        , string status
        , DateTime fromCreateDate
        , DateTime toCreateDate
        , float fromTotalPrice
        , float toTotalPrice
        , string sortByCreateDate
        , string sortByTotalPrice)
    {
        
        //tạo filter
        var filters = new List<FilterDefinition<Ticket>>();
        
        //kiểm tra param userId
        if (!string.IsNullOrEmpty(userId))
        {
            filters.Add(Builders<Ticket>.Filter.Eq(u => u.UserId, userId)); 
        }
        
        //kiểm tra param scheduleId
        if (scheduleId != null && scheduleId.Count > 0)
        {
            filters.Add(Builders<Ticket>.Filter.In(u => u.MovieScheduleId, scheduleId));
        }
        
        //kiểm tra param status
        if (!string.IsNullOrEmpty(status))
        {
            filters.Add(Builders<Ticket>.Filter.Eq(u => u.Status, status));
        }
        
        //kiểm tra param fromCreateDate
        if (fromCreateDate != DateTime.MinValue)
        {
            filters.Add(Builders<Ticket>.Filter.Gte(u => u.CreatedAt, fromCreateDate));
        }
        
        //kiểm tra param toCreateDate
        if (toCreateDate != DateTime.MinValue)
        {
            filters.Add(Builders<Ticket>.Filter.Lte(u => u.CreatedAt, toCreateDate));
        }
        
        //kiểm tra param fromTotalPrice
        if (fromTotalPrice != 0)
        {
            filters.Add(Builders<Ticket>.Filter.Gte(u => u.TotalAmount, fromTotalPrice));
        }
        
        //kiểm tra param toTotalPrice
        if (toTotalPrice != 0)
        {
            filters.Add(Builders<Ticket>.Filter.Lte(u => u.TotalAmount, toTotalPrice));
        }
        
        //kết hợp các filter, nếu ko có filter nào thì dùng filter empty
        var combinedFilter = filters.Count > 0 ? Builders<Ticket>.Filter.And(filters) : Builders<Ticket>.Filter.Empty;
        //tạo sort definition
        SortDefinition<Ticket> sortDefinition = Builders<Ticket>.Sort.Descending(u => u.CreatedAt);
        //sắp xếp
        if (sortByCreateDate == "asc")
        {
            sortDefinition = Builders<Ticket>.Sort.Ascending(u => u.CreatedAt);
        }
        else if(sortByCreateDate == "desc")
        {
            sortDefinition = Builders<Ticket>.Sort.Descending(u => u.CreatedAt);
        }
        if (sortByTotalPrice == "asc")
        {
            sortDefinition = Builders<Ticket>.Sort.Ascending(u => u.TotalAmount);
        }
        else if(sortByTotalPrice == "desc")
        {
            sortDefinition = Builders<Ticket>.Sort.Descending(u => u.TotalAmount);
        }
        //lấy count record
        var totalRecords = await _collection.CountDocumentsAsync(combinedFilter);
        //lấy data theo page và limit
        var records = await _collection
            .Find(combinedFilter) //sử dụng filter
            .Sort(sortDefinition) //sắp xếp
            .Skip((page - 1) * limit)
            .Limit(limit)
            .ToListAsync();
        
        return new Dictionary<string, object>
        {
            {"totalRecords", totalRecords},
            {"records", records},
            {"limit", limit},
            {"page", page}
        };
    }
    
}