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
    
}