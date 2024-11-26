using ReservationService.Entity.Model;
using ReservationService.Repository.MongoDBRepo;

namespace ReservationService.Repository.Interface;

public interface IFoodRepository : IRepository<Food>
{
    //hàm lấy danh sách Food theo id
    Task<List<Food>> GetByFoodIdAsync(List<string> foodIds);
    
    //hàm tìm food nâng cao
    Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , List<string> foodType);
}