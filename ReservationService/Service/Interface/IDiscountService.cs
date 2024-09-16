using ReservationService.Entity.Model;

namespace ReservationService.Service.Interface;

public interface IDiscountService
{
    Task<IEnumerable<Discount>> GetAllAsync();
    Task<Discount> GetByIdAsync(string id);
    Task<Discount> AddAsync(Discount discount);
    Task<Discount> UpdateAsync(string id, Discount discount);
    Task<bool> RemoveAsync(string id);
}