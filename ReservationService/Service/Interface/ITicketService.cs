using ReservationService.Entity.Model;

namespace ReservationService.Service.Interface;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<Dictionary<String, Object>> GetByIdAsync(string id);
    Task<Ticket> AddAsync(Ticket ticket);
    Task<Ticket> UpdateAsync(string id, Ticket ticket);
    Task<bool> RemoveAsync(string id);
    
    //lấy danh sách vé theo id lịch chiếu
    Task<List<Ticket>> GetByScheduleIdAsync(string scheduleId);
}