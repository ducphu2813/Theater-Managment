using Reservation.Domain.Entity;

namespace Reservation.Application.Interfaces.Service;

public interface IFoodService
{
    Task<Dictionary<string, object>> GetAllAsync(int page, int limit);
    Task<Food> GetByIdAsync(string id);
    Task<Food> AddAsync(Food food);
    Task<Food> UpdateAsync(string id, Food food);
    Task<bool> RemoveAsync(string id);
    
    //hàm tìm food nâng cao
    Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , List<string> foodType);
}