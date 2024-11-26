using MongoDB.Bson;
using MongoDB.Driver;
using ReservationService.Context;
using ReservationService.Entity.Model;
using ReservationService.Repository.Interface;
using ReservationService.Repository.MongoDBRepo;

namespace ReservationService.Repository;

public class FoodRepository : MongoDBRepository<Food>, IFoodRepository
{
    public FoodRepository(MongoDBContext context) : base(context, "Food")
    {
    }
    
    //hàm lấy danh sách Food theo id
    public async Task<List<Food>> GetByFoodIdAsync(List<string> foodIds)
    {
        var filter = Builders<Food>.Filter.In("Id", foodIds); //Filter.In để tìm theo 1 danh sách giá trị
        return await _collection.Find(filter).ToListAsync();
    }
    
    //hàm tìm food nâng cao
    public async Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , List<string> foodType)
    {
        // tạo một list để lưu các sub filter
        var subFilters = new List<FilterDefinition<Food>>();

        // param foodType
        if (foodType != null && foodType.Count > 0)
        {
            var foodTypeFilters = foodType.Select(f => Builders<Food>.Filter.Regex(x => x.FoodType, new BsonRegularExpression(f, "i")));
            subFilters.Add(Builders<Food>.Filter.And(foodTypeFilters));
        }
        
        // tạo filter chính
        var filter = Builders<Food>.Filter.And(subFilters);
        
        //lấy total record
        var totalRecord = await _collection.CountDocumentsAsync(filter);
        
        //lấy danh sách food
        var records = await _collection
            .Find(filter)
            .Skip((page - 1) * limit)
            .Limit(limit)
            .ToListAsync();
        
        return new Dictionary<string, object>
        {
            {"totalRecords", totalRecord},
            {"records", records},
            {"limit", limit},
            {"page", page}
        };
    }
    
}