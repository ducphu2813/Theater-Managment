using Reservation.Domain.Entity;

namespace Reservation.Application.Interfaces.Service;

public interface IDiscountService
{
    Task<Dictionary<string, object>> GetAllAsync(int page, int limit);
    Task<Discount> GetByIdAsync(string id);
    Task<Discount> AddAsync(Discount discount);
    Task<Discount> UpdateAsync(string id, Discount discount);
    Task<bool> RemoveAsync(string id);
    
    //hàm tìm nâng cao
    Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit);
}