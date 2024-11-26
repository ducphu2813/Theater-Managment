using MongoDB.Driver;
using ReservationService.Context;
using ReservationService.Entity.Model;
using ReservationService.Repository.Interface;
using ReservationService.Repository.MongoDBRepo;

namespace ReservationService.Repository;

public class DiscountRepository : MongoDBRepository<Discount>, IDiscountRepository
{
    public DiscountRepository(MongoDBContext context) : base(context, "Discount")
    {
        
    }
    
    //hàm lấy danh sách Discount theo FoodType và SeatType
    public async Task<List<Discount>> GetByFoodTypeAndSeatTypeAsync(List<string> foodTypes, List<string> seatTypes)
    {
        var filter = Builders<Discount>.Filter.In("FoodType", foodTypes) & 
                     Builders<Discount>.Filter.In("SeatType", seatTypes);
        return await _collection.Find(filter).ToListAsync();
    }
    
    //hàm tìm nâng cao
    public async Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit)
    {
        //lấy record theo page và limit
        var records = await _collection.Find(_ => true)
            .Skip((page - 1) * limit)
            .Limit(limit)
            .ToListAsync();
        
        //tổng record
        var totalRecords = await _collection.CountDocumentsAsync(_ => true);
        
        return new Dictionary<string, object>
        {
            { "totalRecords", totalRecords },
            { "records", records },
            { "limit", limit },
            { "page", page }
        };
    }
    
}