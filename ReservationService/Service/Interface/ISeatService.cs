using ReservationService.Entity.Model;

namespace ReservationService.Service.Interface;

public interface ISeatService
{
    Task<IEnumerable<Seat>> GetAllAsync();
    Task<Seat> GetByIdAsync(string id);
    Task<Seat> AddAsync(Seat seat);
    Task<Seat> UpdateAsync(string id, Seat seat);
    Task<bool> RemoveAsync(string id);
    
}