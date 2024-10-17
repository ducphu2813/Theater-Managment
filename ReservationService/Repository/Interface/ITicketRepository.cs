using ReservationService.Entity.Model;
using ReservationService.Repository.MongoDBRepo;

namespace ReservationService.Repository.Interface;

public interface ITicketRepository : IRepository<Ticket>
{
    //lấy danh sách vé theo id lịch chiếu
    Task<List<Ticket>> GetByScheduleIdAsync(string scheduleId);
    
    //hàm xóa tất cả vé
    Task<bool> RemoveAllAsync();
    
    //hàm tìm bằng schelude id và seat id
    Task<List<Ticket>> GetByScheduleIdAndSeatIdAsync(string scheduleId, List<string> seatIds);
}