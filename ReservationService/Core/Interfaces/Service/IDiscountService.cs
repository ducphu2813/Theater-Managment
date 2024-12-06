

using ReservationService.Core.Entity.Model;

namespace ReservationService.Core.Interfaces.Service;

public interface IDiscountService
{
    Task<IEnumerable<Discount>> GetAllAsync();
    Task<Discount> GetByIdAsync(string id);
    Task<Discount> AddAsync(Discount discount);
    Task<Discount> UpdateAsync(string id, Discount discount);
    Task<bool> RemoveAsync(string id);
    
    //hàm tìm nâng cao
    Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit);
}