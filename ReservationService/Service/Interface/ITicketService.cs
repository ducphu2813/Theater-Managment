using ReservationService.Entity.Model;

namespace ReservationService.Service.Interface;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<Ticket> GetByIdAsync(string id);
    Task<Ticket> AddAsync(Ticket ticket);
    Task<Ticket> UpdateAsync(string id, Ticket ticket);
    Task<bool> RemoveAsync(string id);
    
}